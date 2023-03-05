using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeploymentPage : PageView
{
    [SerializeField] private UnitManager manager;
    public Button DeploymentButton;
    [SerializeField] private Button startButton;

    private List<GameObject> selection = new List<GameObject>();
    private List<GameObject> ghostsDeployed = new List<GameObject>();

    [SerializeField] private DeploymentPoint mouseFollower;
    [SerializeField] private List<Transform> spawnLocations;
    [SerializeField] private DeployableUnitsPage unitsPage;
    [SerializeField] private List<CompanyIcon> companyIconList = new List<CompanyIcon>();
    [SerializeField] private Transform companyContent;

    [SerializeField] private GameObject companyIconPrefab;
    [SerializeField] private GameObject squadPrefab;

    public CompanyIcon activeCompany;

    CameraPosition positioner;


    private void Start()
    {
        manager = FindObjectOfType<UnitManager>();
    }

    //Change - This needs to load a new data that contains the selected units and their companies
    public void SetupDeploymentPage(SelectionInfo selectedUnits)
    {
        foreach(CompanyInfo company in selectedUnits.companies)
        {
            CompanyIcon icon = Instantiate(companyIconPrefab, companyContent).GetComponent<CompanyIcon>();
            icon.SetupIcon(company, this);
            companyIconList.Add(icon);
        }

        companyContent.GetComponent<RectTransform>().sizeDelta = new Vector2(210 * companyIconList.Count, companyContent.GetComponent<RectTransform>().sizeDelta.y);

        positioner = FindObjectOfType<CameraPosition>();
    }

    public void ToggleDeploymentMenu()
    {
        if (gameObject.activeSelf == true)
        {
            SetPageState(false);
            DestroyGhosts();
            EventSystem.current.SetSelectedGameObject(null);

            return;
        }

        SetPageState(true);
        mouseFollower.ToggleActive(true);
    }

    public void SetDeploymentMenuState(bool state)
    {
        SetPageState(state);
        mouseFollower.ToggleActive(state);

        if (state == false)
        {
            DestroyGhosts();
            return;
        }
    }

    public void ChangeCompany(CompanyIcon newCompany)
    {
        if(newCompany == null)
        {
            unitsPage.SetPageState(false);
            activeCompany = null;
            return;
        }

        if(newCompany == activeCompany)
        {
            activeCompany = null;
            unitsPage.Clear();
            unitsPage.SetPageState(false);
            return;
        }

        unitsPage.SetPageState(true);
        unitsPage.SetActiveCompany(newCompany);
        activeCompany = newCompany;
    }

    public void SpawnGhost(SquadInfo ghost)
    {
        GameObject newGhost;

        if (selection.Count < 4)
        {
            newGhost = Instantiate(squadPrefab, mouseFollower.transform);
            newGhost.transform.localPosition = new Vector3(10 * selection.Count, 0, 0);
            newGhost.GetComponent<GhostUnit>().SetGhost(ghost, manager.equipmentModel);
            selection.Add(newGhost);
            return;
        }
    }

    private void DestroyGhosts()
    {
        foreach (GameObject ghost in selection)
        {
            Destroy(ghost);
        }

        selection.Clear();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (selection.Count > 0)
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1000))
                {
                    if (hit.collider.tag == "Terrain")
                    {
                        SetNewGhostSpawn(hit.point);
                        DestroyGhosts();
                    }
                }
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            DestroyGhosts();
        }

        mouseFollower.transform.rotation = positioner.transform.rotation;

        if(ghostsDeployed.Count > 0)
        {
            startButton.gameObject.SetActive(true);
            return;
        }

        startButton.gameObject.SetActive(false);
    }

    private void SetNewGhostSpawn(Vector3 point)
    {
        foreach(GameObject ghost in selection)
        {
            ghostsDeployed.Add(ghost);
            ghost.transform.parent = null;
            unitsPage.RemoveUsedUnit(ghost.GetComponent<GhostUnit>().Info);
        }

        selection.Clear();
    }

    private void SpawnNewUnit(GameObject ghost)
    {
        GhostUnit ghostUnit = ghost.GetComponent<GhostUnit>();
        GameObject unitPrefab = ghostUnit.UnitPrefab;

        //Change - If we're going to do later deployment, add this for it
        /*if (spawnLocations.Count > 0)
        {
            float distance = -1;
            foreach (Transform vec in spawnLocations)
            {
                float distance2 = Vector3.Distance(point, vec.position);
                if (distance == -1)
                {
                    spawnPosition = vec;
                    distance = distance2;
                    continue;
                }

                if (distance2 < distance)
                {
                    spawnPosition = vec;
                    distance = distance2;
                }
            }
        }*/

        GameObject newGO = Instantiate(unitPrefab, manager.transform);
        SquadObject unit = newGO.GetComponent<SquadObject>();
        newGO.transform.position = ghost.transform.position;
        newGO.transform.rotation = ghost.transform.rotation;
        unit.Initialize(manager, ghostUnit.GetComponent<GhostUnit>().Info);
        unit.SetTargetLocation(ghost.transform.position);

        manager.SpawnPlayerNewUnit(unit);
    }

    public void StartBattle()
    {
        foreach(GameObject ghost in ghostsDeployed)
        {
            SpawnNewUnit(ghost);
            ghost.SetActive(false);
        }

        SetDeploymentMenuState(false);
        DeploymentButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
    }
}
