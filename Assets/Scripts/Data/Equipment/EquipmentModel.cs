using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChapterMaster.Data.Structs;

public class EquipmentModel
{
    public Dictionary<string, WeaponInfo> weapons = new Dictionary<string, WeaponInfo>();
    public Dictionary<string, ArmourInfo> armours = new Dictionary<string, ArmourInfo>();

    public void Load(EquipmentData data)
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
        info.modelObject = (GameObject)Resources.Load(weapon.model);
        info.modelAudio = (AudioClip)Resources.Load(weapon.sound);
        weapons.Add(weapon.weaponName, info);
    }

    public void SetupArmour(Armour armour)
    {
        ArmourInfo info = new ArmourInfo();
        info.data = armour;
        info.modelObject = (GameObject)Resources.Load(armour.model);
        info.modelAudio = (AudioClip)Resources.Load(armour.footStepSound);
        armours.Add(armour.armourName, info);
    }
}
