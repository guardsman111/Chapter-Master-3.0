using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChapterMaster.Data.Structs;
using static ChapterMaster.Data.Enums;

public class BiomeModel : MonoBehaviour
{
    public Dictionary<string, BiomeDef> HotBiomes = new Dictionary<string, BiomeDef>();
    public Dictionary<string, BiomeDef> ColdBiomes = new Dictionary<string, BiomeDef>();
    public Dictionary<string, BiomeDef> GoldilocksBiomes = new Dictionary<string, BiomeDef>();
    public Dictionary<string, OrganismDef> organisms = new Dictionary<string, OrganismDef>();

    public void Load(BiomeData data)
    {
        foreach(BiomeDef biome in data.biomes)
        {
            if (biome.climateTypes.Contains(ClimateType.Hot))
            {
                HotBiomes.Add(biome.biomeName, biome);
            }
            if (biome.climateTypes.Contains(ClimateType.Cold))
            {
                ColdBiomes.Add(biome.biomeName, biome);
            }
            if (biome.climateTypes.Contains(ClimateType.Goldilocks))
            {
                GoldilocksBiomes.Add(biome.biomeName, biome);
            }
        }

        foreach (OrganismDef organism in data.organisms)
        {
            organisms.Add(organism.organismName, organism);
        }
    }
}
