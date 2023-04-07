using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetModel : MonoBehaviour
{
    [SerializeField] private NameTag nameTag;

    private PlanetData data;
    private PlanetSlot slot;
    private Camera Camera;

    private GameObject planetObject;

    public void Initialize(PlanetData planetData, PlanetSlot planetSlot, Camera cam)
    {
        if (data != null)
        {
            Debug.LogWarning($"Planet {data.planetName} already initialized");
            return;
        }

        data = planetData;
        slot = planetSlot;
        Camera = cam;

        GeneratePlanet();
        nameTag.transform.parent = slot.system.transform;
        nameTag.Initialize(data.planetName, Camera);
    }

    private void GeneratePlanet()
    {
        GameObject planet = (GameObject)Resources.Load("Planets/" + data.modelName);

        if (planet == null)
        {
            Debug.LogError("Planet model couldn't be found");
            return;
        }

        planetObject = Instantiate(planet, transform);

        planetObject.transform.localPosition = new Vector3(slot.distance, 0, 0);
        planetObject.transform.localScale = new Vector3(data.scale, data.scale, data.scale);
        int random = Random.Range(-180, 180);
        planetObject.transform.localEulerAngles = new Vector3(0, random, data.rotation);
        random = Random.Range(-180, 180);
        transform.localEulerAngles = new Vector3(0, random, 0);

        planetObject.GetComponent<LightSource>().Sun = slot.system.gameObject;

        StartCoroutine(ToggleExtras());
    }

    public void Update()
    {
        planetObject.transform.localEulerAngles += new Vector3(0, 0.01f * data.spin, 0);
        transform.localEulerAngles += new Vector3(0, 0.01f * data.orbitSpeed, 0);
    }

    public void FixedUpdate()
    {
        nameTag.transform.position = planetObject.transform.position;
        nameTag.RotateTag();
    }

    public IEnumerator ToggleExtras()
    {
        yield return new WaitForSeconds(0.3f);

        if (data.population == 0)
        {
            foreach (Transform child in planetObject.GetComponentsInChildren<Transform>())
            {
                if (child.name == "Planet")
                {
                    child.GetComponent<MeshRenderer>().material.SetFloat("_EnableCities", 0);
                }
            }
        }

        if (data.rings)
        {
            foreach (Transform child in planetObject.GetComponentsInChildren<Transform>())
            {
                if (child.name == "Rings")
                {
                    child.GetComponent<MeshRenderer>().enabled = true;
                }
            }
        }
    }

    public void TogglePlanet(bool state)
    {
        planetObject.SetActive(state);
    }
}
