using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static ChapterMaster.Data.Structs;

public class ChapterPageManager : MonoBehaviour
{
    [SerializeField] private Main main;
    [SerializeField] private Camera menuCamera;

    [SerializeField] private OrganisationPage orgPage;
    [SerializeField] private CompanyPage companyPage;
    [SerializeField] private SquadPage squadPage;
    [SerializeField] private SoldierPage soldierPage;
    [SerializeField] private SelectPage selectPage;

    private CompanyBox currentCompany;
    private SquadBox currentSquad;
    private SoldierBox currentSoldier;

    public EquipmentModel EquipmentModel;

    public void Start()
    {
        main = FindObjectOfType<Main>();
        main.RetrieveChapterOrg(this);
    }

    public void Initialise(EquipmentModel equipmentModel, ChapterModel model, ChapterInfo info)
    {
        EquipmentModel = equipmentModel;
        orgPage.Initialise(model, info);
        companyPage.Initialise();
        squadPage.Initialise();
        soldierPage.Initialise();
        gameObject.SetActive(true);
        menuCamera.gameObject.SetActive(true);
    }

    public void BackToMainMenu()
    {
        main.QuitToMenu();
    }

    public void LoadChapterPage()
    {
        if(currentCompany != null)
        {
            orgPage.ReloadCompany(currentCompany);
            companyPage.Clear();
        }

        orgPage.Load();
        orgPage.gameObject.SetActive(true);
        companyPage.gameObject.SetActive(false);
        selectPage.gameObject.SetActive(false);
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
        if (currentSquad != null)
        {
            companyPage.ReloadSquad(currentSquad);
            squadPage.Clear();
        }

        companyPage.gameObject.SetActive(true);
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
        if(currentSoldier != null)
        {
            squadPage.ReloadSoldier(currentSoldier);
            soldierPage.Clear();
        }

        squadPage.gameObject.SetActive(true);
        soldierPage.gameObject.SetActive(false);
    }

    public void LoadSoldierPage(SoldierModel model, SoldierBox box)
    {
        currentSoldier = box;
        soldierPage.Load(model);
        squadPage.gameObject.SetActive(false);
        soldierPage.gameObject.SetActive(true);
    }

    public void SelectUnitsForBattle(SelectionInfo info)
    {
        selectPage.Initialise(info);
        currentCompany = null;
        selectPage.gameObject.SetActive(true);
    }

    public void GoToBattle(SelectionInfo info)
    {
        main.OpenBattlefield(info);
        gameObject.SetActive(false);
        menuCamera.gameObject.SetActive(false);
    }
}
