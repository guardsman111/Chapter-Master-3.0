using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterPageManager : MonoBehaviour
{
    [SerializeField] private OrganisationPage orgPage;
    [SerializeField] private CompanyPage companyPage;

    public void LoadChapterPage()
    {
        orgPage.Load();
        orgPage.gameObject.SetActive(true);
        companyPage.gameObject.SetActive(false);
    }

    public void LoadCompanyPage(CompanyModel model)
    {
        companyPage.Load(model);
        orgPage.gameObject.SetActive(false);
        companyPage.gameObject.SetActive(true);
    }
}
