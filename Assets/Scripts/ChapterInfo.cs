using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChapterInfo
{
    public SquadInfo hqSquad;
    public SquadInfo churchSquad;
    public SquadInfo librarySquad;
    public SquadInfo medicSquad;
    public SquadInfo armourySquad;
    public List<CompanyInfo> companies;
}
