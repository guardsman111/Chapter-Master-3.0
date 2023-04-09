using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SystemData
{
    public int ID;
    public Vector3 position;
    public string systemName;
    public List<PlanetData> planets;
}
