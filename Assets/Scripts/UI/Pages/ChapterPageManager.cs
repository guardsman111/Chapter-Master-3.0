using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChapterMaster.Data.Structs;

public class ChapterPageManager : MonoBehaviour
{
    [SerializeField] private OrganisationPage orgPage;
    [SerializeField] private CompanyPage companyPage;
    [SerializeField] private SquadPage squadPage;
    [SerializeField] private SoldierPage soldierPage;

    public EquipmentModel EquipmentModel;

    private void Start()
    {
        this.EquipmentModel = new EquipmentModel();
        //Change - Nicer path finding pls
        EquipmentModel.SetupModel(JsonUtility.FromJson<EquipmentData>(Application.streamingAssetsPath + "/Configs/EquipmentData.json"));
    }

    public void LoadChapterPage()
    {
        orgPage.Load();
        orgPage.gameObject.SetActive(true);

        companyPage.Clear();
        companyPage.gameObject.SetActive(false);
    }

    public void LoadCompanyPage(CompanyModel model)
    {
        companyPage.Load(model);
        orgPage.gameObject.SetActive(false);
        companyPage.gameObject.SetActive(true);
    }

    public void BackToCompanyPage()
    {
        companyPage.gameObject.SetActive(true);
        squadPage.Clear();
        squadPage.gameObject.SetActive(false);
    }

    public void LoadSquadPage(SquadModel model)
    {
        squadPage.Load(model);
        companyPage.gameObject.SetActive(false);
        squadPage.gameObject.SetActive(true);
    }

    public void BackToSquadPage()
    {
        squadPage.gameObject.SetActive(true);
        soldierPage.Clear();
        soldierPage.gameObject.SetActive(false);
    }

    public void LoadSoldierPage(SoldierModel model)
    {
        soldierPage.Load(model);
        squadPage.gameObject.SetActive(false);
        soldierPage.gameObject.SetActive(true);
    }
}
