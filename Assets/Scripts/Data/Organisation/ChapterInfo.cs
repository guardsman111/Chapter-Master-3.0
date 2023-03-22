using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChapterMaster.Data.Structs;

[Serializable]
public class ChapterInfo
{
    public SquadInfo hqSquad;
    public SquadInfo churchSquad;
    public SquadInfo librarySquad;
    public SquadInfo medicSquad;
    public SquadInfo armourySquad;
    public List<CompanyInfo> companies;

    public string patternName;
    public List<MaterialDef> colours;
}
