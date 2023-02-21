using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CompanyInfo
{
    public string CompanyName;
    public string CompanyNickname;
    public int CompanyID;
    public List<SquadInfo> Squads;
}
