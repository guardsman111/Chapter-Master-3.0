using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierModel
{
    private SquadModel soldierModel;
    private SoldierInfo soldierData;

    public SoldierInfo SoldierData { get => soldierData; }

    public void Load(SoldierInfo data, SquadModel model)
    {
        soldierModel = model;
        soldierData = data;
    }
}
