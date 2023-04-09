using ChapterMaster;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static ChapterMaster.Data.Structs;

public class Model
{
    public BiomeModel BiomeModel;
    public EquipmentModel EquipmentModel;
    public SaveData SavedData;
    public ChapterModel ChapterModel;
    public NameLocalisationStructure localisation;

    private GenerationScript generator = new GenerationScript();

    private SelectionInfo selectedInfo;

    // Start is called before the first frame update
    public void Initialise()
    {
        //Change - Nicer path finding pls

        if (!File.Exists(Application.streamingAssetsPath + "/Configs/EquipmentData.json"))
        {
            Debug.LogError("Equipment data not found aborting");
            return;
        }
        this.EquipmentModel = new EquipmentModel();
        string jsonToRead = File.ReadAllText(Application.streamingAssetsPath + "/Configs/EquipmentData.json");
        EquipmentModel.Load(JsonUtility.FromJson<EquipmentData>(jsonToRead));

        if (!File.Exists(Application.streamingAssetsPath + "/BiomeData.json"))
        {
            Debug.LogError("Biome data not found aborting");
            return;
        }
        this.BiomeModel = new BiomeModel();
        jsonToRead = File.ReadAllText(Application.streamingAssetsPath + "/BiomeData.json");
        BiomeModel.Load(JsonUtility.FromJson<BiomeData>(jsonToRead));

        if (File.Exists(Application.streamingAssetsPath + "/Configs/Localisation.json"))
        {
            jsonToRead = File.ReadAllText(Application.streamingAssetsPath + "/Configs/Localisation.json");
            localisation = JsonUtility.FromJson<NameLocalisationStructure>(jsonToRead);
        }

        if (!File.Exists(Application.streamingAssetsPath + "/Save.json"))
        {
            Debug.LogError("Save data not found generating");
            generator.Initialize(localisation);
        }
        this.ChapterModel = new ChapterModel();
        this.SavedData = new SaveData();
        jsonToRead = File.ReadAllText(Application.streamingAssetsPath + "/Save.json");
        this.SavedData = JsonUtility.FromJson<SaveData>(jsonToRead);
        this.ChapterModel.Load(SavedData.chapter);
    }

    public void SetSelectedInfo(SelectionInfo info)
    {
        selectedInfo = info;
    }

    public SelectionInfo GetSelectedInfo()
    {
        if(selectedInfo == null)
        {
            Debug.LogError("Nothing selected");
            return null;
        }
        return selectedInfo;
    }

    public void SaveData()
    {
        string path = Application.streamingAssetsPath + "/Save.json";

        string json = JsonUtility.ToJson(SavedData);
        File.WriteAllText(path, json);
    }
}
