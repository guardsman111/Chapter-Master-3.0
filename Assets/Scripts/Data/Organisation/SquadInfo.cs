using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChapterMaster.Data.Enums;

[Serializable]
public class SquadInfo
{
    public string SquadName;
    public SquadType SquadType;
    public int SquadID;

    public List<SoldierInfo> Soldiers;
}
