using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChapterMaster.Data.Enums;

namespace ChapterMaster.Data
{
    public class Structs
    {
        [Serializable]
        public struct NameLocalisationStructure
        {
            public List<string> firstNames;
            public List<string> secondNames;
            public List<string> starNames;
        }

        [Serializable]
        public struct EquipmentData
        {
            public List<Weapon> weapons;
            public List<Armour> armours;
        }

        [Serializable]
        public struct Weapon
        {
            public string weaponName;
            public string type;
            public float accuracy;
            public float range;
            public float fireRate;
            public float magCapacity;
            public float damage;
            public float piercing;
            public string model;
            public string sound;
        }

        [Serializable]
        public struct Armour
        {
            public string armourName;
            public string type;
            public float protectionLevel;
            public float coverage;
            public string model;
            public string footStepSound;
        }

        [Serializable]
        public struct ArmourPattern
        {
            public string patternName;
            public List<SkinnedMeshRenderer> primaryMeshes;
            public List<SkinnedMeshRenderer> secondaryMeshes;
        }

        [Serializable]
        public struct MaterialDef
        {
            public string materialName;
            public Color colour;
            public float metallic;
            public float smoothness;
        }

        [Serializable]
        public struct BiomeDef
        {
            public string biomeName;
            public List<ClimateType> climateTypes;
            public int maxPopulation;
            public int maxWealth;
            public bool habitable;
            public List<string> availableFauna;
            public List<string> availableFlora;
            public string modelName;
            public float minScale;
            public float maxScale;
        }

        [Serializable]
        public struct OrganismDef
        {
            public string organismName;
            public string organismDescription;
            public float populationEffect;
            public float wealthEffect;
            public bool isFauna;
        }

        /*
        public struct Weapon
        {
            public string weaponName; - Name
            public string type; - Type from Enum - needs to match or be set as primary
            public float accuracy; - Value from 100% (cannot miss unless marine impaired)
            public float range; - No limit, how far it can shoot
            public float fireRate; - How long between each shot and sound
            public float magCapacity; - How many shots before reload
            public float damage; - 50 probably max, which is instant ded on everything, like a nuke
            public float damage; - Value from 10, knocks armour level down by 1 
            public string model; - The name of the model file it uses
            public string sound; - The name of the sound file it uses
        }

        public struct Armour
        {
            public string armourName; - Name
            public string type; Type from enum - needs to match or be set as Power
            public float protectionLevel; - Value from 12. Chance to avoid damage completely
            public float coverage; - Value from 100%, how much of the body is covered - chance to avoid armour entirely
            public string model; - The name of the model file it uses
            public string footStepSound; - The name of the sound file it uses
        }*/
    }
}
