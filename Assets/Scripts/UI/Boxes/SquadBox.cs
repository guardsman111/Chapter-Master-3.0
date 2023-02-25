using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SquadBox : MonoBehaviour
{
    private ChapterPageManager manager;

    [SerializeField] private TMP_InputField squadName;
    [SerializeField] private TextMeshProUGUI squadType;
    [SerializeField] private TextMeshProUGUI squadSoldierInfo;

    private SquadModel squadModel;

    public void LoadSquad(SquadModel squad, ChapterPageManager manager)
    {
        this.manager = manager;

        squadModel = squad;
        squadName.text = squad.SquadData.SquadName;
        squadType.text = squad.SquadData.SquadType.ToString() + " squad";
        squadName.onValueChanged.AddListener(SetNewName);

        CalculateSoldierComposition();
    }

    public void ReloadSquad()
    {
        squadName.text = squadModel.SquadData.SquadName;
        squadType.text = squadModel.SquadData.SquadType.ToString() + " squad";

        CalculateSoldierComposition();
    }

    private void SetNewName(string newName)
    {
        squadModel.SquadData.SquadName = newName;
    }

    private void CalculateSoldierComposition()
    {
        string compositionString = "";

        foreach(SoldierModel soldier in squadModel.SoldierDataDictionary.Values)
        {
            compositionString += soldier.SoldierData.soldierName + "\n";
        }

        squadSoldierInfo.text = compositionString;
    }

    public void SetSquadPage()
    {
        manager.LoadSquadPage(squadModel, this);
    }
}
