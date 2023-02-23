using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ChapterMaster.Data.Enums;

public class SoldierPage : MonoBehaviour
{
    [SerializeField] private ChapterPageManager manager;

    [SerializeField] private TMP_InputField soldierName;
    [SerializeField] private Dropdown soldierDesignation;
    [SerializeField] private Dropdown weaponDropdown;
    [SerializeField] private Dropdown armourDropdown;

    SoldierModel soldierModel;

    private void Start()
    {
        soldierDesignation.ClearOptions();
        soldierDesignation.AddOptions(Enum.GetNames(typeof(SoldierDesignation)).ToList());
        List<string> newData = new List<string>();

        foreach(WeaponInfo weapon in manager.EquipmentModel.weapons)
        {
            newData.Add(weapon.data.weaponName);
        }

        weaponDropdown.ClearOptions();
        weaponDropdown.AddOptions(newData);
        newData.Clear();

        foreach (ArmourInfo armour in manager.EquipmentModel.armours)
        {
            newData.Add(armour.data.armourName);
        }

        armourDropdown.ClearOptions();
        armourDropdown.AddOptions(newData);
        newData.Clear();

        gameObject.SetActive(false);
    }

    public void Load(SoldierModel soldier)
    {
        soldierModel = soldier;
        soldierName.text = soldier.SoldierData.firstName + " " + soldier.SoldierData.secondName;
    }

    public void Clear()
    {
        //Empty
    }

    public void Back()
    {
        manager.BackToSquadPage();
    }
}
