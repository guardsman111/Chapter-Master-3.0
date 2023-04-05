using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ChapterMaster.Data.Enums;
using static ChapterMaster.Data.Structs;

public class UnitObject : MonoBehaviour
{
    [SerializeField] private EquipmentModel equipmentModel;
    [SerializeField] private Base_Behaviour behaviour;

    [SerializeField] private Transform primaryParent;
    [SerializeField] private Transform secondaryParent;
    [SerializeField] private Transform meleeParent;
    [SerializeField] private Transform armourParent;
    [SerializeField] private GameObject armourObject;
    [SerializeField] private UnitCollider colliderUnit;
    public UnitCollider Collider { get => colliderUnit; }
    [SerializeField] private SquadObject parentSquad;
    public SquadObject ParentSquad { get => parentSquad; }

    [SerializeField] private SquadObject targetSquad;

    private Dictionary<WeaponType, WeaponScript> weapons = new Dictionary<WeaponType, WeaponScript>();

    private float health;
    private bool isDead = false;

    private SoldierInfo data;
    public SoldierInfo Data { get => data; }

    [SerializeField] private LayerMask TerrainLayerMask;

    public float turnSpeed = 5;

    public ArmourColourer colourer;

    private Coroutine fireCorout;

    private bool isOrgScreen;

    public void Clear()
    {
        foreach(WeaponScript weapon in weapons.Values)
        {
            if(weapon != null)
            {
                Destroy(weapon.gameObject);
            }
        }

        weapons.Clear();
        Destroy(armourObject);
        data = null;
    }

    private void Update()
    {
        if(isOrgScreen)
        {
            return;
        }
        if(isDead)
        {
            return;
        }

        if(parentSquad.isMoving == true)
        {
            AngleUnitToFloor();
        }

        FaceClosestEnemy();
    }

    public void Load(SoldierModel soldier, EquipmentModel model, SquadObject parent, string patternName, bool isOrgScreen = false)
    {
        this.isOrgScreen = isOrgScreen;

        if(model == null)
        {
            Debug.LogError("Equipment Model was null, do a thing");
            return;
        }

        parentSquad = parent;
        equipmentModel = model;

        if (soldier.SoldierData.armour == null)
        {
            Debug.LogError($"Unable to load armour type {soldier.SoldierData.armour}");
            return;
        }

        armourObject = Instantiate(equipmentModel.armours[soldier.SoldierData.armour].modelObject, armourParent);
        primaryParent = armourObject.GetComponent<ArmourModelScript>().primaryHome;
        secondaryParent = armourObject.GetComponent<ArmourModelScript>().secondaryHome;
        meleeParent = armourObject.GetComponent<ArmourModelScript>().meleeHome;
        colourer = armourObject.GetComponent<ArmourColourer>();
        colourer.SetArmourPattern(patternName);

        if (soldier.SoldierData.primaryWeapon != "")
        {
            GameObject primary = Instantiate(equipmentModel.weapons[soldier.SoldierData.primaryWeapon].modelObject, primaryParent);
            WeaponScript newWeapon = primary.GetComponent<WeaponScript>();
            weapons.Add(WeaponType.Primary, newWeapon);
        }

        if (soldier.SoldierData.secondaryWeapon != "")
        {
            GameObject secondary = Instantiate(equipmentModel.weapons[soldier.SoldierData.secondaryWeapon].modelObject, secondaryParent);
            WeaponScript newWeapon = secondary.GetComponent<WeaponScript>();
            weapons.Add(WeaponType.Secondary, newWeapon);
        }

        if (soldier.SoldierData.meleeWeapon != "")
        {
            GameObject melee = Instantiate(equipmentModel.weapons[soldier.SoldierData.meleeWeapon].modelObject, meleeParent);
            WeaponScript newWeapon = melee.GetComponent<WeaponScript>();
            weapons.Add(WeaponType.Melee, newWeapon);
        }

        data = soldier.SoldierData;
        health = 50;
    }

    public void ChangeWeapon(WeaponType type, string weapon)
    {
        if (weapons.ContainsKey(type))
        {
            if (weapons[type] != null)
            {
                Destroy(weapons[type].gameObject);
            }
            weapons.Remove(type);
        }

        Transform parent = null;

        switch (type)
        {
            case WeaponType.Primary:
                parent = primaryParent;
                break;
            case WeaponType.Secondary:
                parent = secondaryParent;
                break;
            case WeaponType.Melee:
                parent = meleeParent;
                break;
        }

        GameObject weaponObj = Instantiate(equipmentModel.weapons[weapon].modelObject, parent);
        WeaponScript newWeapon = weaponObj.GetComponent<WeaponScript>();
        weapons.Add(type, newWeapon);
    }

    public void ChangeArmour(string armour)
    {
        if (armourObject != null)
        {
            Destroy(armourObject);
        }

        armourObject = Instantiate(equipmentModel.weapons[armour].modelObject, armourParent);
        colourer = armourObject.GetComponent<ArmourColourer>();
    }

    public void ToggleMesh(bool value)
    {
        armourParent.gameObject.SetActive(value);
    }

    public void SetTarget(SquadObject newTarget)
    {
        targetSquad = newTarget;
        if(fireCorout != null)
        {
            StopCoroutine(fireCorout);
        }
        fireCorout = StartCoroutine(FireAtEnemy());
    }

    public void AngleUnitToFloor()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), -transform.up, out hit, 100, TerrainLayerMask))
        {
            Quaternion newRot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, 25 * Time.deltaTime);
        }
    }

    public void FaceClosestEnemy()
    {
        if(targetSquad != null)
        {
            Vector3 lookVec = targetSquad.transform.position - transform.position;
            lookVec.y = 0;
            Quaternion lookRot = Quaternion.LookRotation(lookVec);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, turnSpeed * Time.deltaTime);
            return;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, parentSquad.transform.rotation, turnSpeed * Time.deltaTime);
    }

    public IEnumerator FireAtEnemy()
    {
        if (targetSquad != null)
        {
            float distance = Vector3.Distance(transform.position, targetSquad.transform.position);
            if (isDead)
            {
                yield break;
            }

            if (distance < equipmentModel.weapons[data.primaryWeapon].data.range)
            {
                TakeShot(equipmentModel.weapons[data.primaryWeapon]);
                yield return new WaitForSeconds(equipmentModel.weapons[data.primaryWeapon].data.fireRate);
                StopCoroutine(fireCorout);
                fireCorout = StartCoroutine(FireAtEnemy());
                yield break;
            }

            if (distance < equipmentModel.weapons[data.secondaryWeapon].data.range)
            {
                TakeShot(equipmentModel.weapons[data.secondaryWeapon]);
                yield return new WaitForSeconds(equipmentModel.weapons[data.secondaryWeapon].data.fireRate);
                StopCoroutine(fireCorout);
                fireCorout = StartCoroutine(FireAtEnemy());
                yield break;
            }

            yield return new WaitForSeconds(0.1f);
            StopCoroutine(fireCorout);
            fireCorout = StartCoroutine(FireAtEnemy());
        }
    }

    public void TakeShot(WeaponInfo shootingWeapon)
    {
        if (targetSquad != null)
        {
            //Chance to hit squad
            int rand = UnityEngine.Random.Range(0, 100);

            if (rand < shootingWeapon.data.accuracy)
            {
                targetSquad.TakeHit(shootingWeapon);
                return;
            }
        }
    }

    public void TakeHit(WeaponInfo shootingWeapon)
    {
        //Chance to hit armour
        int rand = UnityEngine.Random.Range(0, 100);

        if (rand < equipmentModel.armours[data.armour].data.coverage)
        {
            float newArmour = equipmentModel.armours[data.armour].data.protectionLevel - shootingWeapon.data.piercing;

            rand = UnityEngine.Random.Range(0, 12);
            if (rand > newArmour)
            {
                TakeDamage(shootingWeapon.data.damage);
            }
            return;
        }
        
        TakeDamage(shootingWeapon.data.damage);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (!isDead)
        {
            parentSquad.RemoveMember(this);
            behaviour.Killed();
            isDead = true;
            parentSquad = null;
            if (fireCorout != null)
            {
                StopCoroutine(fireCorout);
            }
            Destroy(GetComponent<BoxCollider>());
        }
    }
}
