using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlanetData 
{
    public string planetName;
    public int slotID;
    public string biome;
    public string climate;
    public int population;
    public int wealth;
    public List<string> fauna;
    public List<string> flora;
    public bool rings;
    public float orbitSpeed;
    public float spin;
    public float rotation;
    public float scale;
    public string modelName;
}
