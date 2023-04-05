using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static ChapterMaster.Data.Structs;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private DeploymentPage deploymentPage;

    public EquipmentModel equipmentModel;
    public ChapterModel chapterModel;

    private Dictionary<int, SquadObject> playerUnits = new Dictionary<int, SquadObject>();
    public Dictionary<int, SquadObject> PlayerUnits
    {
        get => playerUnits;
        private set { playerUnits = value; }
    }

    private Dictionary<int, SquadObject> hostileUnits = new Dictionary<int, SquadObject>();
    public Dictionary<int, SquadObject> HostileUnits
    {
        get => hostileUnits;
        private set { hostileUnits = value; }
    }

    private Dictionary<int, SquadObject> playerVisibleUnits = new Dictionary<int, SquadObject>();
    public Dictionary<int, SquadObject> PlayerVisibleUnits
    {
        get => playerVisibleUnits;
        private set { playerVisibleUnits = value; }
    }

    private Dictionary<int, SquadObject> hostileVisibleUnits = new Dictionary<int, SquadObject>();
    public Dictionary<int, SquadObject> HostileVisibleUnits
    {
        get => hostileVisibleUnits;
        private set { hostileVisibleUnits = value; }
    }

    [SerializeField] private List<Transform> objectiveLocations;

    public List<SquadObject> playerHardStartUnits = new List<SquadObject>();
    public List<SquadObject> hostileHardStartUnits = new List<SquadObject>();

    [SerializeField] private LayerMask hostileLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask terrainLayer;

    public bool testingHardStart = false;

    public void Initialize(EquipmentModel equipModel)
    {
        equipmentModel = equipModel;
    }

    public void SelectUnitsForBattle(SelectionInfo selection, ChapterModel model)
    {
        //Change - This should eventually be removed
        /*if (testingHardStart)
        {
            string data;

            string equipPath = Application.streamingAssetsPath + "/Configs/EquipmentData.json";

            if (!File.Exists(equipPath))
            {
                Debug.LogError("Equipment data not found aborting");
                return;
            }

            data = File.ReadAllText(equipPath);
            equipmentModel.Load(JsonUtility.FromJson<EquipmentData>(data));

            string savePath = Application.streamingAssetsPath + "/Save.json";
            ChapterInfo info = new ChapterInfo();

            if (!File.Exists(savePath))
            {
                Debug.LogError("Save data not found aborting");
                return;
            }

            data = File.ReadAllText(savePath);
            info = JsonUtility.FromJson<ChapterInfo>(data);

            for (int i = 0; i < playerHardStartUnits.Count; i++)
            {
                if (playerInfo.Squads.Count < i)
                {
                    Debug.LogWarning("More player hard start units than squads in data");
                    break;
                }

                playerHardStartUnits[i].Initialize(this, playerInfo.Squads[i]);
            }

            for (int i = 0; i < hostileHardStartUnits.Count; i++)
            {
                if (hostileInfo.Squads.Count < i)
                {
                    Debug.LogWarning("More hostile hard start units than squads in data");
                    break;
                }

                hostileHardStartUnits[i].Initialize(this, hostileInfo.Squads[i]);
            }
        }*/

        //Change - This needs to load a new data that contains the selected units and their companies
        chapterModel = model;
        deploymentPage.SetupDeploymentPage(selection);
    }

    public void AddUnit(SquadObject unit, bool isPlayer)
    {
        if (isPlayer)
        {
            Debug.Log($"Unit added is {unit.data.SquadName} with ID {unit.ID}");
            PlayerUnits.Add(unit.ID, unit);

            return;
        }

        HostileUnits.Add(unit.ID, unit);
        unit.AddComponent<SimpleAI>().Initialize(unit, objectiveLocations);

        unit.ToggleMesh(false);
    }

    public void RemoveUnit(SquadObject unit, bool isPlayer)
    {
        if (isPlayer)
        {
            PlayerUnits.Remove(unit.ID);
            PlayerVisibleUnits.Remove(unit.ID);

            return;
        }

        HostileUnits.Remove(unit.ID);
        HostileVisibleUnits.Remove(unit.ID);
    }

    public void MakeUnitVisible(SquadObject unit, bool isPlayer)
    {
        if (isPlayer)
        {
            if (PlayerVisibleUnits.ContainsKey(unit.ID) == false)
            {
                PlayerVisibleUnits.Add(unit.ID, unit);
            }

            return;
        }

        if (HostileVisibleUnits.ContainsKey(unit.ID) == false)
        {
            HostileVisibleUnits.Add(unit.ID, unit);
        }

        unit.ToggleMesh(true);
    }

    public void MakeUnitNotVisible(SquadObject unit, bool isPlayer)
    {
        if (isPlayer)
        {
            PlayerVisibleUnits.Remove(unit.ID);

            foreach (SquadObject spotter in unit.EnemiesSpotting)
            {
                spotter.SpottedEnemyNotSpottedAnymore(unit);
            }

            return;
        }

        foreach (SquadObject spotter in unit.EnemiesSpotting)
        {
            spotter.SpottedEnemyNotSpottedAnymore(unit);
        }

        HostileVisibleUnits.Remove(unit.ID);

        unit.ToggleMesh(false);
    }

    public SquadObject CheckClosestVisibleEnemy(SquadObject viewer, bool isPlayer)
    {
        int closestID = 0;

        Dictionary<int, SquadObject> enemyDict = null;

        // If it's a player's unit check the enemy's list
        if (isPlayer)
        {
            if (HostileVisibleUnits.Count != 0)
            {
                enemyDict = HostileVisibleUnits;
            }
        }
        else
        {
            // If it's an enemy unit check the player's list
            if (PlayerVisibleUnits.Count != 0)
            {
                enemyDict = PlayerVisibleUnits;
            }
        }

        if (enemyDict == null)
        {
            //Debug.Log("No visible units");
            return null;
        }

        closestID = FindClosestUnit(viewer, enemyDict);

        if (closestID == -1)
        {
            //Debug.LogError("Error finding visible enemy - returned no enemy");
            return null;
        }

        return enemyDict[closestID];
    }

    public int FindClosestUnit(SquadObject viewer, Dictionary<int, SquadObject> enemyDict)
    {
        int closestID = -1;

        float shortestDistance = 0;
        //Change - make this more efficient by weeding out targets that are too far away before raycasting
        foreach (SquadObject enemy in enemyDict.Values)
        {
            RaycastHit[] hits;

            Vector3 direction = enemy.transform.position - viewer.transform.position;

            LayerMask mask = playerLayer;
            if (viewer.IsPlayer)
            {
                mask = hostileLayer;
            }

            hits = Physics.RaycastAll(viewer.transform.position + new Vector3(0,1,0), direction, viewer.Stats.opticsRange);
            float targetDistance = Vector3.Distance(enemy.transform.position, viewer.transform.position);

            if (hits.Length > 0)
            {
                hits = ArrangeHitsByDistance(hits);

                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.gameObject.layer == viewer.gameObject.layer)
                    {
                        continue;
                    }
                    if (hit.collider.tag == "Collider")
                    {
                        //Debug.DrawLine(viewer.transform.position, hit.point, Color.red, 1.0f);
                        if (hit.collider.GetComponent<UnitCollider>().Unit == enemy)
                        {
                            float distance = Vector3.Distance(viewer.transform.position, enemy.transform.position);
                            if (distance < shortestDistance || shortestDistance == 0)
                            {
                                shortestDistance = distance;
                                closestID = enemy.ID;
                                break;
                            }
                        }
                    }
                    if (hit.collider.tag == "Unit")
                    {
                        //Debug.DrawLine(viewer.transform.position, hit.point, Color.red, 1.0f);
                        if (hit.collider.GetComponent<SquadObject>() == enemy)
                        {
                            float distance = Vector3.Distance(viewer.transform.position, enemy.transform.position);
                            if (distance < shortestDistance || shortestDistance == 0)
                            {
                                shortestDistance = distance;
                                closestID = enemy.ID;
                                break;
                            }
                        }
                    }
                    if (hit.collider.tag == "Terrain")
                    {
                        //Debug.DrawLine(viewer.transform.position, hit.point, Color.yellow, 1.0f);
                        break;
                    }
                }
            }
        }
        return closestID;
    }

    private RaycastHit[] ArrangeHitsByDistance(RaycastHit[] hits)
    {
        RaycastHit[] newHits = hits.ToList<RaycastHit>().OrderBy(x => x.distance).ToArray();

        return newHits;
    }

    internal void SpawnPlayerNewUnit(SquadObject unit)
    {
        unit.SetPlayer(true);

        AddUnit(unit, true);
    }
}
