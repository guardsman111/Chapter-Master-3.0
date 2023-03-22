using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ChapterMaster.Data.Enums;
using static ChapterMaster.Data.Structs;

public class SoldierPage : MonoBehaviour
{
    [SerializeField] private ChapterPageManager manager;
    [SerializeField] private UnitObject soldierDisplay;

    [SerializeField] private TMP_InputField soldierName;
    [SerializeField] private Dropdown soldierDesignation;
    [SerializeField] private Dropdown primaryWeaponDropdown;
    [SerializeField] private Dropdown secondaryWeaponDropdown;
    [SerializeField] private Dropdown meleeWeaponDropdown;
    [SerializeField] private Dropdown armourDropdown;
    List<string> primaries = new List<string>();
    List<string> secondaries = new List<string>();
    List<string> melees = new List<string>();
    List<string> armours = new List<string>();

    SoldierModel soldierModel;

    public void Initialise()
    {
        soldierName.onValueChanged.AddListener(SetNewName);
        soldierDesignation.onValueChanged.AddListener(SetNewDesignation);
        soldierDesignation.ClearOptions();
        soldierDesignation.AddOptions(Enum.GetNames(typeof(SoldierDesignation)).ToList());

        foreach (WeaponInfo weapon in manager.EquipmentModel.weapons.Values)
        {
            switch (weapon.data.type)
            {
                case "Primary":
                    primaries.Add(weapon.data.weaponName);
                    break;
                case "Secondary":
                    primaries.Add(weapon.data.weaponName);
                    secondaries.Add(weapon.data.weaponName);
                    break;
                case "Melee":
                    primaries.Add(weapon.data.weaponName);
                    secondaries.Add(weapon.data.weaponName);
                    melees.Add(weapon.data.weaponName);
                    break;
            }
        }

        primaryWeaponDropdown.ClearOptions();
        secondaryWeaponDropdown.ClearOptions();
        meleeWeaponDropdown.ClearOptions();
        primaryWeaponDropdown.AddOptions(primaries);
        secondaryWeaponDropdown.AddOptions(secondaries);
        meleeWeaponDropdown.AddOptions(melees);

        foreach (ArmourInfo armour in manager.EquipmentModel.armours.Values)
        {
            armours.Add(armour.data.armourName);
        }

        armourDropdown.ClearOptions();
        armourDropdown.AddOptions(armours);

        primaryWeaponDropdown.onValueChanged.AddListener(ChangePrimaryWeapon);
        secondaryWeaponDropdown.onValueChanged.AddListener(ChangeSecondaryWeapon);
        meleeWeaponDropdown.onValueChanged.AddListener(ChangeMeleeWeapon);

        armourDropdown.onValueChanged.AddListener(ChangeArmour);

        gameObject.SetActive(false);
    }

    private void SetNewName(string newName)
    {
        soldierModel.SoldierData.soldierName = newName;
    }
    private void SetNewDesignation(int newDesignation)
    {
        soldierModel.SoldierData.designation = ((SoldierDesignation)newDesignation).ToString();
    }

    public void Load(SoldierModel soldier)
    {
        soldierModel = soldier;
        soldierName.text = soldier.SoldierData.soldierName;
        soldierDisplay.Load(soldier, manager.EquipmentModel, null, manager.GetArmourPattern(), true);
        soldierDesignation.value = (int)Enum.Parse(typeof(SoldierDesignation), soldier.SoldierData.designation);
        primaryWeaponDropdown.value = primaries.IndexOf(soldier.SoldierData.primaryWeapon);
        secondaryWeaponDropdown.value = secondaries.IndexOf(soldier.SoldierData.secondaryWeapon);
        meleeWeaponDropdown.value = melees.IndexOf(soldier.SoldierData.meleeWeapon);
        armourDropdown.value =  armours.IndexOf(soldier.SoldierData.armour);
    }

    public void Clear()
    {
        soldierDisplay.Clear();
    }

    public void Back()
    {
        manager.BackToSquadPage();
    }

    private void ChangePrimaryWeapon(int weapon)
    {
        soldierDisplay.ChangeWeapon(WeaponType.Primary, primaries[weapon]);
        soldierModel.SoldierData.primaryWeapon = primaries[weapon];
    }

    private void ChangeSecondaryWeapon(int weapon)
    {
        soldierDisplay.ChangeWeapon(WeaponType.Secondary, secondaries[weapon]);
        soldierModel.SoldierData.secondaryWeapon = secondaries[weapon];
    }

    private void ChangeMeleeWeapon(int weapon)
    {
        soldierDisplay.ChangeWeapon(WeaponType.Melee, melees[weapon]);
        soldierModel.SoldierData.meleeWeapon = melees[weapon];
    }

    private void ChangeArmour(int armour)
    {
        soldierDisplay.ChangeArmour(armours[armour]);
        soldierModel.SoldierData.armour = armours[armour];
    }
}
