using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CompanyBox : MonoBehaviour
{
    private ChapterPageManager manager;

    [SerializeField] private TMP_InputField companyName;
    [SerializeField] private TMP_InputField companyNickname;
    [SerializeField] private TextMeshProUGUI companySquadInfo;

    private CompanyModel companyModel;

    public void LoadCompany(CompanyModel company, ChapterPageManager manager)
    {
        companyModel = company;
        this.manager = manager;
        companyName.text = company.CompanyData.CompanyName;
        companyName.onValueChanged.AddListener(SetNewName);
        companyNickname.onValueChanged.AddListener(SetNewNickname);

        CalculateSquadComposition();
    }

    private void SetNewName(string newName)
    {
        companyModel.CompanyData.CompanyName = newName;
    }

    private void SetNewNickname(string newName)
    {
        companyModel.CompanyData.CompanyNickname = newName;
    }

    private void CalculateSquadComposition()
    {
        Dictionary<string, int> composition = new Dictionary<string, int>();

        foreach(SquadModel squad in companyModel.SquadDataDictionary.Values)
        {
            if(composition.ContainsKey(squad.SquadData.SquadType.ToString()))
            {
                composition[squad.SquadData.SquadType.ToString()] += 1;
                continue;
            }

            composition.Add(squad.SquadData.SquadType.ToString(), 1);
        }

        string compositionString = "";

        foreach(KeyValuePair<string, int> type in composition)
        {
            compositionString += $"{type.Value} x {type.Key} Squads" + "\n";
        }

        companySquadInfo.text = compositionString;
    }

    public void SetCompanyPage()
    {
        manager.LoadCompanyPage(companyModel);
    }
}
