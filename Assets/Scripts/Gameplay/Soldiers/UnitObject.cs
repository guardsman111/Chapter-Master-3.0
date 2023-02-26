using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChapterMaster.Data.Enums;
using static ChapterMaster.Data.Structs;

public class UnitObject : MonoBehaviour
{
    [SerializeField] private EquipmentModel equipmentModel;

    [SerializeField] private Transform primaryParent;
    [SerializeField] private Transform secondaryParent;
    [SerializeField] private Transform meleeParent;
    [SerializeField] private Transform armourParent;
    [SerializeField] private GameObject armourObject;
    [SerializeField] private UnitCollider collider;
    public UnitCollider Collider { get => collider; }

    Dictionary<WeaponType, GameObject> weapons = new Dictionary<WeaponType, GameObject>();

    private SoldierInfo data;
    public SoldierInfo Data { get => data; }

    [SerializeField] private LayerMask TerrainLayerMask;

    public void Clear()
    {
        foreach(GameObject weapon in weapons.Values)
        {
            Destroy(weapon);
        }

        weapons.Clear();
        Destroy(armourObject);
        data = null;
    }

    private void Update()
    {
        AngleUnitToFloor();
    }

    public void Load(SoldierModel soldier, EquipmentModel model)
    {
        if(model == null)
        {
            Debug.LogError("Equipment Model was null, do a thing");
            return;
        }

        equipmentModel = model;

        if (soldier.SoldierData.armour == null)
        {
            Debug.Log($"Unable to load armour type {soldier.SoldierData.armour}");
            return;
        }

        armourObject = Instantiate(equipmentModel.armours[soldier.SoldierData.armour].modelObject, armourParent);
        primaryParent = armourObject.GetComponent<ArmourModelScript>().primaryHome;
        secondaryParent = armourObject.GetComponent<ArmourModelScript>().secondaryHome;
        meleeParent = armourObject.GetComponent<ArmourModelScript>().meleeHome;

        if (soldier.SoldierData.primaryWeapon != "")
        {
            GameObject primary = Instantiate(equipmentModel.weapons[soldier.SoldierData.primaryWeapon].modelObject, primaryParent);
            weapons.Add(WeaponType.Primary, primary);
        }

        if (soldier.SoldierData.secondaryWeapon != "")
        {
            GameObject secondary = Instantiate(equipmentModel.weapons[soldier.SoldierData.secondaryWeapon].modelObject, secondaryParent);
            weapons.Add(WeaponType.Secondary, secondary);
        }

        if (soldier.SoldierData.meleeWeapon != "")
        {
            GameObject melee = Instantiate(equipmentModel.weapons[soldier.SoldierData.meleeWeapon].modelObject, meleeParent);
            weapons.Add(WeaponType.Melee, melee);
        }

        data = soldier.SoldierData;
    }

    public void ChangeWeapon(WeaponType type, string weapon)
    {
        if (weapons.ContainsKey(type))
        {
            Destroy(weapons[type]);
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

        GameObject newWeapon = Instantiate(equipmentModel.weapons[weapon].modelObject, parent);
        weapons.Add(type, newWeapon);
    }

    public void ChangeArmour(string armour)
    {
        if (armourObject != null)
        {
            Destroy(armourObject);
        }

        armourObject = Instantiate(equipmentModel.weapons[armour].modelObject, armourParent);
    }

    public void ToggleMesh(bool value)
    {
        armourParent.gameObject.SetActive(value);
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
}
