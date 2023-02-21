using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrganisationPage : MonoBehaviour
{
    [SerializeField] private ChapterPageManager manager;
    [SerializeField] private RectTransform companiesParent;
    [SerializeField] private GameObject companyBoxPrefab;
    
    [SerializeField] private SquadBox hqBox;
    [SerializeField] private SquadBox churchBox;
    [SerializeField] private SquadBox libraryBox;
    [SerializeField] private SquadBox medicBox;
    [SerializeField] private SquadBox armouryBox;
    [SerializeField] private Dictionary<int, CompanyBox> companyBoxes;

    ChapterInfo info;
    ChapterModel chapterModel;

    public void Load()
    {
        Load(chapterModel, info);
    }

    public void Load(ChapterModel model, ChapterInfo info)
    {
        if(info == null)
        {
            this.info = info;
        }

        if (chapterModel == null)
        {
            chapterModel = model;
        }

        companyBoxes = new Dictionary<int, CompanyBox>();

        foreach(CompanyModel company in model.CompanyDataDictionary.Values)
        {
            CompanyBox companyBox = Instantiate(companyBoxPrefab, companiesParent).GetComponent<CompanyBox>();
            companyBox.LoadCompany(company, manager);
            companyBoxes.Add(company.CompanyData.CompanyID, companyBox);
        }

        companiesParent.sizeDelta = new Vector2(425 * companyBoxes.Count, 0);
    }

    public void PrintData()
    {
        foreach(CompanyInfo company in info.companies)
        {
            Debug.LogWarning($"Company called {company.CompanyName} nicknamed {company.CompanyNickname}");
        }
    }
}
