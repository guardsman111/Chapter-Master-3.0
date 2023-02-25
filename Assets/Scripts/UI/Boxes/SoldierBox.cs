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
        soldierName.text = soldier.SoldierData.soldierName;

        CalculateEquipmentComposition();
    }

    public void ReloadSoldier(SoldierBox soldier)
    {
        soldierName.text = soldier.SoldierModel.SoldierData.soldierName;

        CalculateEquipmentComposition();
    }

    private void CalculateEquipmentComposition()
    {
        string compositionString = "";

        compositionString += "Primary: " + SoldierModel.SoldierData.primaryWeapon + "\n";
        compositionString += "Secondary: " + SoldierModel.SoldierData.secondaryWeapon + "\n";
        compositionString += "Melee: " + SoldierModel.SoldierData.meleeWeapon + "\n";
        compositionString += "Armour: " + SoldierModel.SoldierData.armour + "\n";

        soldierInfo.text = compositionString;
    }

    public void SetSoldierPage()
    {
        manager.LoadSoldierPage(SoldierModel, this);
    }
}
