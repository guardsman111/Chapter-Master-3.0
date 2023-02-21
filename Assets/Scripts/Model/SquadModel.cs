using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadModel : MonoBehaviour
{
    private CompanyModel companyModel;
    private SquadInfo squadData;
    public SquadInfo SquadData { get => squadData; }

    public void Load(SquadInfo data, CompanyModel model)
    {
        companyModel = model;
        squadData = data;
    }
}
