using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationTestScript : MonoBehaviour
{
    public OrganisationPage page;

    void Start()
    {
        ChapterInfo info = new ChapterInfo();
        info.companies = new List<CompanyInfo>();

        for(int i = 0; i < 10; i++)
        {
            CompanyInfo company = new CompanyInfo();
            company.CompanyName = "Company " + i;
            company.CompanyNickname = "Nickname " + i;
            company.CompanyID = i;
            company.Squads = new List<SquadInfo>();

            CreateSquads(company);

            info.companies.Add(company);
        }

        ChapterModel model = new ChapterModel();
        model.Load(info);

        page.Load(model, info);
    }

    private void CreateSquads(CompanyInfo company)
    {

        for (int i = 0; i < 10; i++)
        {
            SquadInfo Squad = new SquadInfo();
            Squad.SquadName = "Squad " + i;
            Squad.SquadID = i;

            company.Squads.Add(Squad);
        }
    }
}
