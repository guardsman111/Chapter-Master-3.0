using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static ChapterMaster.Data.Structs;

public class SectorModel : MonoBehaviour
{
    public Model model;
    public GameObject systemPrefab;
    [SerializeField] private Camera cam;

    [SerializeField] List<BiomeDef> biomes;
    [SerializeField] List<OrganismDef> organisms;

    private Dictionary<int, SystemModel> systems = new Dictionary<int, SystemModel>();
    private List<SystemData> systemDatas = new List<SystemData>();
    private bool planetsHidden = true;

    private void Start()
    {
        Main main = FindObjectOfType<Main>();
        main.RetrieveGalaxyInfo(this);
    }

    public void Initialize(int numberOfStars, Model newModel = null, SectorData data = null)
    {
        if (newModel == null)
        {
        }
        else
        {
            model = newModel;
        }

        if(data != null)
        {
            foreach(SystemData system in data.systems)
            {
                SystemModel sModel = Instantiate(systemPrefab, transform).GetComponent<SystemModel>();

                sModel.transform.position = system.position;
                systems.Add(system.ID, sModel);

                sModel.Initialize(this, system.systemName, camera: cam, system: system);
            }

            systemDatas = data.systems;
            return;
        }

        for (int i = 0; i < numberOfStars; i++)
        {
            SystemModel system = Instantiate(systemPrefab, transform).GetComponent<SystemModel>();

            int randomX = Random.Range(-50000, 50000);
            int randomZ = Random.Range(-50000, 50000);

            Vector3 position = new Vector3(randomX, 0, randomZ);

            bool tooClose = true;
            int count = 0;

            while (tooClose)
            {
                count += 1;
                tooClose = false;
                foreach (SystemModel star in systems.Values)
                {
                    if(Vector3.Distance(star.transform.position, position) < 3000)
                    {
                        tooClose = true;
                        randomX = Random.Range(-50000, 50000);
                        randomZ = Random.Range(-50000, 50000);

                        position = new Vector3(randomX, 0, randomZ);
                        break;
                    }
                }
                if(count > 10)
                {
                    Debug.LogWarning("Couldn't fit system");
                    break;
                }
            }

            int randomName = Random.Range(0, model.localisation.starNames.Count);
            string systemName = model.localisation.starNames[randomName];


            system.transform.position = position;
            systems.Add(i, system);

            system.Initialize(this, systemName, camera: cam, ID: i);
            systemDatas.Add(system.ReturnDataToSave());
        }

        model.SavedData.sector.systems = systemDatas;
        model.SaveData();
    }

    public void TogglePlanets(bool state)
    {
        if(planetsHidden == state)
        {
            return; 
        }

        planetsHidden = state;

        foreach(SystemModel system in systems.Values)
        {
            system.TogglePlanets(state);
        }
    }
}
