using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    public void Initialise(ChapterModel model, ChapterInfo chapterInfo)
    {
        Load(model, chapterInfo);
    }

    public void Load()
    {
        Load(chapterModel, info);
    }

    public void Load(ChapterModel model, ChapterInfo chapterInfo)
    {
        if(info == null)
        {
            info = chapterInfo;
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

        companiesParent.sizeDelta = new Vector2((425 * companyBoxes.Count) + 25, 0);
    }

    public void Clear()
    {
        foreach (CompanyBox box in companyBoxes.Values)
        {
            Destroy(box.gameObject);
        }

        companyBoxes.Clear();
    }

    public void ReloadCompany(CompanyBox company)
    {
        if (companyBoxes.ContainsValue(company))
        {
            company.ReloadCompany();
        }
    }

    public void BackToMainMenu()
    {
        manager.BackToMainMenu();
    }

    public void SelectUnitsForBattle()
    {
        SelectionInfo info = new SelectionInfo();
        info.companies = chapterModel.ChapterDataPublic.companies.ToList();
        manager.SelectUnitsForBattle(info);
    }
}
