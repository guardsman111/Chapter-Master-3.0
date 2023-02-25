using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static ChapterMaster.Data.Structs;
using static UnityEngine.GraphicsBuffer;

public class SquadObject : MonoBehaviour
{
    [SerializeField] private UnitManager manager;
    [SerializeField] private AIPath pathfinder;
    public int ID { get; protected set; }
    [SerializeField] private bool isPlayer = false;
    public bool IsPlayer { get => isPlayer; }

    private List<UnitObject> members = new List<UnitObject>();

    protected SquadObject closestEnemy;
    protected SquadObject target;
    protected List<SquadObject> enemiesSpotting = new List<SquadObject>();
    public List<SquadObject> EnemiesSpotting { get => enemiesSpotting; }
    public Vector3 targetLocation;

    [SerializeField] private LayerMask TerrainLayerMask;
    private bool isDead = false;

    public SquadStats Stats { get; private set; }
    private SquadInfo data;

    private Coroutine scanCoroutine = null;

    public void Initialize(UnitManager newManager = null, SquadInfo info = null)
    {
        if (newManager != null)
        {
            manager = newManager;
        }

        data = info;

        ID = Random.Range(0, 100000);
        if (isPlayer)
        {
            while (manager.PlayerUnits.ContainsKey(ID))
            {
                ID = Random.Range(0, 100000);
            }

            gameObject.layer = 6;
        }

        if (!isPlayer)
        {
            while (manager.HostileUnits.ContainsKey(ID))
            {
                ID = Random.Range(0, 100000);
            }

            gameObject.layer = 7;
        }


        if (targetLocation == null || targetLocation == Vector3.zero)
        {
            pathfinder.destination = transform.position;
            targetLocation = transform.position;
        }

        Stats = new SquadStats
        {
            members = info.Soldiers.Count,
            liveMembers = info.Soldiers.Count
        };

        foreach (SoldierInfo soldier in info.Soldiers)
        {
            GameObject soldierObject;

            if (IsPlayer)
            {
                soldierObject = Instantiate((GameObject)Resources.Load("Space Marine"), transform);
            }
            else
            {
                soldierObject = Instantiate((GameObject)Resources.Load("Traitor Marine"), transform);
            }

            soldierObject.transform.position += new Vector3(0, 0, 1 * info.Soldiers.IndexOf(soldier));
            SoldierModel soldierModel = new SoldierModel();
            soldierModel.Load(soldier, null);
            if (!IsPlayer)
            {
                soldierModel.SoldierData.armour = "MK7Traitor";
            }
            UnitObject soldierUnit = soldierObject.GetComponent<UnitObject>();
            members.Add(soldierUnit);

            soldierUnit.Load(soldierModel, manager.equipmentModel);

            if(soldier.speed < Stats.maxSpeed)
            {
                Stats.maxSpeed = soldier.speed;
            }
            if (soldier.opticsRange > Stats.opticsRange)
            {
                Stats.opticsRange = soldier.opticsRange;
            }
        }

        //Change - Once dynamic battle start in, this needs to be == or the unit will throw an error
        if (newManager != null)
        {
            manager.AddUnit(this, IsPlayer);
        }

        if (Stats.maxSpeed == 0)
        {
            Stats.maxSpeed = 1;
        }
        pathfinder.maxSpeed = Stats.maxSpeed;

        scanCoroutine = StartCoroutine(Scan(1.0f));
    }

    public void SetPlayer(bool value)
    {
        isPlayer = value;
    }

    public void SetTargetEnemy(object unit)
    {
        //Empty
    }

    public void SetTargetLocation(Vector3 point)
    {
        pathfinder.destination = point;
    }

    public void ToggleMesh(bool value)
    {
        foreach(UnitObject member in members)
        {
            member.ToggleMesh(value);
        }
    }

    // Scans for visible enemies and marks a visible enemy as it's target
    private IEnumerator Scan(float delay)
    {
        while (Stats.liveMembers > 0)
        {
            yield return new WaitForSeconds(delay);
            if (IsPlayer)
            {
                Spotting.FindVisibleEnemies(this, manager.HostileUnits);
            }
            else
            {
                Spotting.FindVisibleEnemies(this, manager.PlayerUnits);
            }

            if (CheckStillSpotted() == false)
            {
                manager.MakeUnitNotVisible(this, isPlayer);
                foreach (SquadObject unit in enemiesSpotting)
                {
                    unit.SpottedEnemyNotSpottedAnymore(this);
                }
                enemiesSpotting.Clear();
            }

            if (target == null)
            {
                // gets the closest visible enemy as the default target
                SquadObject currentClosestEnemy = manager.CheckClosestVisibleEnemy(this, isPlayer);
                if (currentClosestEnemy == null)
                {
                    ForgetTarget();
                }
                if (currentClosestEnemy != closestEnemy)
                {
                    closestEnemy = currentClosestEnemy;
                    /*foreach (UnitWeapon weapon in weapons)
                    {
                        weapon.ChangeTarget(currentClosestEnemy);
                    }*/
                }
            }
        }
    }


    // Checks if this unit is already spotted by an enemy
    public bool CheckAlreadySpotted(SquadObject spotter)
    {
        if (enemiesSpotting.Contains(spotter))
        {
            return true;
        }

        return false;
    }

    // Check this unit is still being spotted by any enemies
    public bool CheckStillSpotted()
    {
        foreach (SquadObject unit in enemiesSpotting)
        {
            if (Spotting.CheckCanSeeSpotted(unit, this) == true)
            {
                return true;
            }
        }

        return false;
    }

    // Makes enemy units visible to the opposing side
    public void MakeVisible(SquadObject spottingUnit, bool ispotterPlayer)
    {
        if (ispotterPlayer != isPlayer)
        {
            manager.MakeUnitVisible(this, isPlayer);
            enemiesSpotting.Add(spottingUnit);
        }
    }

    // An enemy that we were spotting is no longer spotted - forget it as a target
    public void SpottedEnemyNotSpottedAnymore(SquadObject unit)
    {
        if (closestEnemy == unit)
        {
           /* foreach (UnitWeapon weapon in weapons)
            {
                if (weapon.TargetUnit == closestEnemy)
                {
                    weapon.ChangeTarget(null);
                }
            }*/
            closestEnemy = null;
        }
    }

    // If there are no more enemies visible we will forget the target we last had - If it's not visible we don't need to worry about it!
    public void ForgetTarget()
    {
        /*foreach (UnitWeapon weapon in weapons)
        {
            if (weapon.TargetUnit == closestEnemy && weapon.TargetUnit != null)
            {
                weapon.ChangeTarget(null);
            }
        }*/
        closestEnemy = null;
    }

    // Reports the target
    public void SpottingEnemyDestroyed(SquadObject unit)
    {
        enemiesSpotting.Remove(unit);
        /*foreach (UnitWeapon weapon in weapons)
        {
            if (weapon.TargetUnit == unit)
            {
                weapon.ChangeTarget(null);
            }
        }*/
    }

    // Future use - when the target unit is no longer visible
    public void TargetLost()
    {
        target = null;
    }
}
