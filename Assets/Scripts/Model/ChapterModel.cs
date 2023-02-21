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
        Debug.Log($"Creating Companies");

        foreach (CompanyInfo company in data.companies)
        {
            Debug.Log($"Creating Company {company.CompanyName}");
            CompanyModel model = new CompanyModel();
            model.Load(company, this);
            CompanyDataDictionary.Add(company.CompanyID, model);
        }
    }
}
