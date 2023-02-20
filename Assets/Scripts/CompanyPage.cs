using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanyPage : MonoBehaviour
{
    private CompanyModel companyModel;

    public void Load(CompanyModel company)
    {
        companyModel = company;

        foreach (SquadModel squad in company.SquadDataDictionary.Values)
        {
            SquadBox squadBox = new SquadBox();
            squadBox.LoadSquad(squad);
        }
    }
}
