using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static ChapterMaster.Data.Structs;

public class ChapterPageManager : MonoBehaviour
{
    [SerializeField] private OrganisationPage orgPage;
    [SerializeField] private CompanyPage companyPage;
    [SerializeField] private SquadPage squadPage;
    [SerializeField] private SoldierPage soldierPage;

    private CompanyBox currentCompany;
    private SquadBox currentSquad;
    private SoldierBox currentSoldier;

    public EquipmentModel EquipmentModel;

    public void Initialise(EquipmentModel equipmentModel, ChapterModel model, ChapterInfo info)
    {
        EquipmentModel = equipmentModel;
        orgPage.Initialise(model, info);
        companyPage.Initialise();
        squadPage.Initialise();
        soldierPage.Initialise();
    }

    public void LoadChapterPage()
    {
        orgPage.ReloadCompany(currentCompany);
        orgPage.Load();
        orgPage.gameObject.SetActive(true);

        companyPage.Clear();
        companyPage.gameObject.SetActive(false);
    }

    public void LoadCompanyPage(CompanyModel model, CompanyBox box)
    {
        currentCompany = box;
        companyPage.Load(model);
        orgPage.gameObject.SetActive(false);
        companyPage.gameObject.SetActive(true);
    }

    public void BackToCompanyPage()
    {
        companyPage.ReloadSquad(currentSquad);
        companyPage.gameObject.SetActive(true);
        squadPage.Clear();
        squadPage.gameObject.SetActive(false);
    }

    public void LoadSquadPage(SquadModel model, SquadBox box)
    {
        currentSquad = box;
        squadPage.Load(model);
        companyPage.gameObject.SetActive(false);
        squadPage.gameObject.SetActive(true);
    }

    public void BackToSquadPage()
    {
        squadPage.ReloadSoldier(currentSoldier);
        squadPage.gameObject.SetActive(true);
        soldierPage.Clear();
        soldierPage.gameObject.SetActive(false);
    }

    public void LoadSoldierPage(SoldierModel model, SoldierBox box)
    {
        currentSoldier = box;
        soldierPage.Load(model);
        squadPage.gameObject.SetActive(false);
        soldierPage.gameObject.SetActive(true);
    }
}
