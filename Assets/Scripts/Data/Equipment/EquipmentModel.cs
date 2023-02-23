using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChapterMaster.Data.Structs;

public class EquipmentModel
{
    public List<WeaponInfo> weapons = new List<WeaponInfo>();
    public List<ArmourInfo> armours = new List<ArmourInfo>();

    public void SetupModel(EquipmentData data)
    {
        foreach(Weapon weapon in data.weapons)
        {
            SetupWeapon(weapon);
        }

        foreach (Armour armour in data.armours)
        {
            SetupArmour(armour);
        }
    }

    public void SetupWeapon(Weapon weapon)
    {
        WeaponInfo info = new WeaponInfo();
        info.data = weapon;
    }

    public void SetupArmour(Armour armour)
    {
        ArmourInfo info = new ArmourInfo();
        info.data = armour;
    }
}
