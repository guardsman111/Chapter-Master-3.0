using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SoldierInfo
{
    public int soldierID;
    public string designation;
    public string soldierName;

    public string primaryWeapon;
    public string secondaryWeapon;
    public string meleeWeapon;

    public string armour;

    public float speed = 5;
    public float opticsRange = 50;
}
