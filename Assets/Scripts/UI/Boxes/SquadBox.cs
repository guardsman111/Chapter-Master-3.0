using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SquadBox : MonoBehaviour
{
    [SerializeField] private TMP_InputField squadName;
    [SerializeField] private TextMeshProUGUI squadType;
    [SerializeField] private TextMeshProUGUI squadSoldierInfo;

    private SquadModel SquadModel;

    public void LoadSquad(SquadModel squad, ChapterPageManager pageManager)
    {
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
        Dictionary<string, int> composition = new Dictionary<string, int>();

        string compositionString = "";

        foreach (KeyValuePair<string, int> type in composition)
        {
            compositionString += $"{type.Value} x {type.Key} Squads" + "\n";
        }

        squadSoldierInfo.text = compositionString;
    }
}
