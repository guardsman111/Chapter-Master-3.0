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

    Dictionary<WeaponType, GameObject> weapons = new Dictionary<WeaponType, GameObject>();

    public void Clear()
    {
        foreach(GameObject weapon in weapons.Values)
        {
            Destroy(weapon);
        }

        weapons.Clear();
        Destroy(armourObject);
    }

    public void Load(SoldierModel soldier, EquipmentModel model)
    {
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
}
