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

    private SquadModel SquadModel;

    public void LoadSquad(SquadModel squad, ChapterPageManager manager)
    {
        this.manager = manager;

        SquadModel = squad;
        squadName.text = squad.SquadData.SquadName;
        squadType.text = squad.SquadData.SquadType.ToString() + " squad";
        squadName.onValueChanged.AddListener(SetNewName);

        CalculateSoldierComposition();
    }

    private void SetNewName(string newName)
    {
        SquadModel.SquadData.SquadName = newName;
    }

    private void CalculateSoldierComposition()
    {
        string compositionString = "";

        foreach(SoldierModel soldier in SquadModel.SoldierDataDictionary.Values)
        {
            compositionString += soldier.SoldierData.firstName.ToString() + " " + soldier.SoldierData.secondName.ToString() + "\n";
        }

        squadSoldierInfo.text = compositionString;
    }

    public void SetSquadPage()
    {
        manager.LoadSquadPage(SquadModel);
    }
}
