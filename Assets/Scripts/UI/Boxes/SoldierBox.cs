using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static ChapterMaster.Data.Enums;

public class SoldierBox : MonoBehaviour
{
    private ChapterPageManager manager;

    [SerializeField] private TMP_InputField soldierName;
    [SerializeField] private TextMeshProUGUI soldierInfo;

    private SoldierModel SoldierModel;

    public void LoadSoldier(SoldierModel soldier, ChapterPageManager manager)
    {
        this.manager = manager;

        SoldierModel = soldier;
        soldierName.text = soldier.SoldierData.firstName + " " + soldier.SoldierData.secondName;

        CalculateEquipmentComposition();
    }

    private void CalculateEquipmentComposition()
    {
        string compositionString = "";

        compositionString += "Primary Weapon: " + SoldierModel.SoldierData.primaryWeapon + "\n";
        compositionString += "Secondary Weapon: " + SoldierModel.SoldierData.secondaryWeapon + "\n";
        compositionString += "Melee Weapon: " + SoldierModel.SoldierData.meleeWeapon + "\n";
        compositionString += "Armour Mark: " + SoldierModel.SoldierData.armour + "\n";

        soldierInfo.text = compositionString;
    }

    public void SetSoldierPage()
    {
        manager.LoadSoldierPage(SoldierModel);
    }
}
