using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static ChapterMaster.Data.Structs;

public class SectorModel : MonoBehaviour
{
    public BiomeModel BiomeModel;
    public SystemModel system;

    [SerializeField] List<BiomeDef> biomes;
    [SerializeField] List<OrganismDef> organisms;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        BiomeData biomeData = new BiomeData();
        biomeData.biomes = biomes;
        biomeData.organisms = organisms;

        string path = Application.streamingAssetsPath + "/BiomeData.json";

        string json = JsonUtility.ToJson(biomeData);
        File.WriteAllText(path, json);

        if (!File.Exists(Application.streamingAssetsPath + "/BiomeData.json"))
        {
            Debug.LogError("Biome data not found aborting");
            return;
        }
        this.BiomeModel = new BiomeModel();
        string jsonToRead = File.ReadAllText(Application.streamingAssetsPath + "/BiomeData.json");
        BiomeModel.Load(JsonUtility.FromJson<BiomeData>(jsonToRead));

        system.Initialize(this);
    }
}
