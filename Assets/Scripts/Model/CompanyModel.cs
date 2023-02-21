using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanyModel
{
    private ChapterModel chapterModel;
    private CompanyInfo companyData;
    public CompanyInfo CompanyData { get => companyData; }
    public Dictionary<int, SquadModel> SquadDataDictionary;

    public void Load(CompanyInfo data, ChapterModel model)
    {
        chapterModel = model;
        companyData = data;
        SquadDataDictionary = new Dictionary<int, SquadModel>();

        foreach (SquadInfo squad in data.Squads)
        {
            SquadModel squadModel = new SquadModel();
            squadModel.Load(squad, this);
            SquadDataDictionary.Add(squad.SquadID, squadModel);
        }
    }
}
