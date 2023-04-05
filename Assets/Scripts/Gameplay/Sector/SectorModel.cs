using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static ChapterMaster.Data.Structs;

public class SectorModel : MonoBehaviour
{
    public Model model;
    public GameObject systemPrefab;
    [SerializeField] private Camera cam;

    [SerializeField] List<BiomeDef> biomes;
    [SerializeField] List<OrganismDef> organisms;

    private Dictionary<int, SystemModel> systems = new Dictionary<int, SystemModel>();

    private void Start()
    {
        Main main = FindObjectOfType<Main>();
        main.RetrieveGalaxyInfo(this);
    }

    public void Initialize(int numberOfStars, Model newModel = null)
    {
        if (newModel == null)
        {
        }
        else
        {
            model = newModel;
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

            system.Camera = cam;
            system.Initialize(this, systemName);
        }
    }
}
