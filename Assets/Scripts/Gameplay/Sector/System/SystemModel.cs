using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static ChapterMaster.Data.Structs;
using static ChapterMaster.Data.Enums;
using TMPro;
using UnityEngine.XR;

public class SystemModel : MonoBehaviour
{
    [SerializeField] private SectorModel model;
    [SerializeField] private List<PlanetSlot> planets;

    [SerializeField] private GameObject planetPrefab;

    private SystemData data;

    public void Initialize(SectorModel sector, SystemData system = null)
    {
        model = sector;
        if(system != null)
        {
            data = system;
            LoadPlanets(data.planets);
        }

        GeneratePlanets();
    }

    private void GeneratePlanets()
    {
        int planetCount = Random.Range(0, planets.Count); 

        if(planetCount == 0)
        {
            return;
        }

        List<int> positions = new List<int>();

        for (int i = 0; i < planetCount; i++)
        {
            int pos = Random.Range(0, planets.Count);
            if (positions.Contains(pos))
            {
                while (positions.Contains(pos))
                {
                    pos += 1;
                    if (pos > planets.Count - 1)
                    {
                        pos = 0;
                    }
                }
                positions.Add(pos);
            }
            else
            {
                positions.Add(pos);
            }

            foreach (PlanetSlot slot in planets)
            {
                if (planets.IndexOf(slot) != pos)
                {
                    continue;
                }

                slot.Initialize(this);
                slot.planet = Instantiate(planetPrefab, slot.transform).GetComponent<PlanetModel>();

                PlanetData planet = new PlanetData();
                planet.planetName = "New Planet ";
                planet.slotID = planets.IndexOf(slot);
                planet.climate = slot.temperature.ToString();

                switch (slot.temperature)
                {
                    case ClimateType.Hot:
                        SetupPlanetBiome(slot, planet, model.BiomeModel.HotBiomes.Values.ToList());
                        break;
                    case ClimateType.Cold:
                        SetupPlanetBiome(slot, planet, model.BiomeModel.ColdBiomes.Values.ToList());
                        break;
                    case ClimateType.Goldilocks:
                        SetupPlanetBiome(slot, planet, model.BiomeModel.GoldilocksBiomes.Values.ToList());
                        break;
                }

                float rotationSpeed = 0;
                while (rotationSpeed == 0)
                {
                    rotationSpeed = Random.Range(-5, 5);
                }
                planet.orbitSpeed = rotationSpeed;

                rotationSpeed = 0;
                while (rotationSpeed == 0)
                {
                    rotationSpeed = Random.Range(-5, 5);
                }
                planet.spin = rotationSpeed;

                float rotationAngle = Random.Range(-90, 90);
                planet.rotation = rotationAngle;

                planet.rings = false;
                int random = Random.Range(0, 100);
                if (random < 20)
                {
                    planet.rings = true;
                }

                slot.planet.Initialize(planet, slot);
            }
        }
    }

    private void SetupPlanetBiome(PlanetSlot slot, PlanetData planet, List<BiomeDef> biomes)
    {
        int random = Random.Range(0, biomes.Count);

        planet.biome = biomes[random].biomeName;
        planet.population = 0;

        if (slot.habitable && biomes[random].habitable)
        {
            planet.population = Random.Range(-10000, biomes[random].maxPopulation);
        }
        else if (slot.habitable)
        {
            planet.population = Random.Range(-10000, biomes[random].maxPopulation) / 100;
        }

        if(planet.population <= 0)
        {
            planet.population = 0;
            planet.wealth = Random.Range(0, biomes[random].maxWealth);
        }

        SetupOrganisms(planet, biomes[random]);

        planet.scale = Random.Range(biomes[random].minScale, biomes[random].maxScale);
        planet.modelName = biomes[random].modelName;
    }

    private void SetupOrganisms(PlanetData planet, BiomeDef biome)
    {
        int startPopulation = planet.population;
        int startWealth = planet.wealth;

        if (planet.fauna == null)
        {
            planet.fauna = new List<string>();
        }

        if (planet.flora == null)
        {
            planet.flora = new List<string>();
        }

        foreach (string fauna in biome.availableFauna)
        {
            if (!model.BiomeModel.organisms.ContainsKey(fauna))
            {
                continue;
            }

            int random = Random.Range(0, 100);

            if(random < 50)
            {
                continue;
            }

            OrganismDef organism = model.BiomeModel.organisms[fauna];

            planet.population += (int)(startPopulation * (organism.populationEffect - 1));
            planet.wealth += (int)(startWealth * (organism.wealthEffect - 1));

            planet.fauna.Add(organism.organismName + " - " + organism.organismDescription);
        }

        foreach (string flora in biome.availableFlora)
        {
            if (!model.BiomeModel.organisms.ContainsKey(flora))
            {
                continue;
            }

            OrganismDef organism = model.BiomeModel.organisms[flora];

            planet.population += (int)(startPopulation * (organism.populationEffect - 1));
            planet.wealth += (int)(startWealth * (organism.wealthEffect - 1));

            planet.flora.Add(organism.organismName + " - " + organism.organismDescription);
        }
    }

    private void LoadPlanets(List<PlanetData> systemPlanets)
    {
        if (planets.Count >= systemPlanets.Count)
        {
            foreach (PlanetData planet in systemPlanets)
            {
                planets[planet.slotID].planet = Instantiate(planetPrefab, planets[planet.slotID].transform).GetComponent<PlanetModel>();
                planets[planet.slotID].planet.Initialize(planet, planets[planet.slotID]);
            }
        }
        else
        {
            Debug.LogError("More planets passed in than slots! ");
        }
    }
}
