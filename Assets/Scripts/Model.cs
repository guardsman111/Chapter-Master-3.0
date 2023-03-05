using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static ChapterMaster.Data.Structs;

public class Model
{
    public EquipmentModel EquipmentModel;
    public ChapterModel ChapterModel;

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


        if (!File.Exists(Application.streamingAssetsPath + "/Save.json"))
        {
            Debug.LogError("Save data not found aborting");
            return;
        }
        this.ChapterModel = new ChapterModel();
        jsonToRead = File.ReadAllText(Application.streamingAssetsPath + "/Save.json");
        ChapterModel.Load(JsonUtility.FromJson<ChapterInfo>(jsonToRead));
    }

    public void SaveData()
    {
        ChapterModel.Save();
    }
}
