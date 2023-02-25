using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterModel
{
    private ChapterInfo ChapterData;
    public Dictionary<int, CompanyModel> CompanyDataDictionary;

    public void Load(ChapterInfo data)
    {
        ChapterData = data;
        CompanyDataDictionary = new Dictionary<int, CompanyModel>();

        foreach (CompanyInfo company in data.companies)
        {
            CompanyModel model = new CompanyModel();
            model.Load(company, this);
            CompanyDataDictionary.Add(company.CompanyID, model);
        }
    }
}
