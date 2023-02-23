using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChapterMaster.Data
{
    public class Structs
    {
        [Serializable]
        public struct NameLocalisationStructure
        {
            public List<string> firstNames;
            public List<string> secondNames;
        }

        public struct WeaponData
        {
            public List<Weapon> weapons;
        }

        [Serializable]
        public struct Weapon
        {
            public string weaponName;
            public float accuracy;
            public float fireRate;
            public float damage;
            public string model;
            public string sound;
        }
    }
}
