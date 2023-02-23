using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SquadModel
{
    private CompanyModel companyModel;
    private SquadInfo squadData;

    public SquadInfo SquadData { get => squadData; }

    public Dictionary<int, SoldierModel> SoldierDataDictionary = new Dictionary<int, SoldierModel>();

    public void Load(SquadInfo data, CompanyModel model)
    {
        companyModel = model;
        squadData = data;

        foreach(SoldierInfo soldier in data.Soldiers)
        {
            SoldierModel soldierModel = new SoldierModel();
            soldierModel.Load(soldier, this);
            SoldierDataDictionary.Add(soldier.soldierID, soldierModel);
        }
    }
}
