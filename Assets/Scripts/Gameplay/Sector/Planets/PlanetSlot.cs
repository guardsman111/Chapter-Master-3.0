using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChapterMaster.Data.Enums;

public class PlanetSlot : MonoBehaviour
{
    public SystemModel system;
    public PlanetModel planet;

    public int distance;

    public ClimateType temperature;
    public bool habitable;

    public void Initialize(SystemModel model)
    {
        system = model;
    }
}
