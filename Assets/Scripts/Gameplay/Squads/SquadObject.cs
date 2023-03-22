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
    private List<GameObject> memberObjects = new List<GameObject>();

    protected SquadObject closestEnemy;
    protected SquadObject target;
    protected List<SquadObject> enemiesSpotting = new List<SquadObject>();
    public List<SquadObject> EnemiesSpotting { get => enemiesSpotting; }
    public Vector3 targetLocation;
    public bool isMoving { get; private set; } = false;

    public SquadStats Stats { get; private set; }
    public SquadInfo data;

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

        int count = 1;

        foreach (SoldierInfo soldier in info.Soldiers)
        {
            GameObject soldierObject;


            //Change - Needs to just load the model once we have enemies specifically spawning in
            if (IsPlayer)
            {
                soldierObject = Instantiate((GameObject)Resources.Load("Space Marine"), transform);
            }
            else
            {
                soldierObject = Instantiate((GameObject)Resources.Load("Traitor Marine"), transform);
            }

            soldierObject.transform.position = CirclePosition(transform.position, 3.0f, count);
            SoldierModel soldierModel = new SoldierModel();
            soldierModel.Load(soldier, null);

            if (!IsPlayer)
            {
                soldierModel.SoldierData.armour = "MK7Traitor";
            }

            UnitObject soldierUnit = soldierObject.GetComponent<UnitObject>();
            members.Add(soldierUnit);
            memberObjects.Add(soldierObject);

            soldierUnit.Load(soldierModel, manager.equipmentModel, this, manager.chapterModel.ChapterDataPublic.patternName);

            if(soldier.speed < Stats.maxSpeed)
            {
                Stats.maxSpeed = soldier.speed;
            }
            if (soldier.opticsRange > Stats.opticsRange)
            {
                Stats.opticsRange = soldier.opticsRange;
            }

            Base_Behaviour behaviour = soldierObject.GetComponent<Base_Behaviour>();

            behaviour.SetTarget(this);
            count++;
        }

        SetMemberBoids();

        if (Stats.maxSpeed == 0)
        {
            Stats.maxSpeed = 5;
        }
        pathfinder.maxSpeed = Stats.maxSpeed;

        scanCoroutine = StartCoroutine(Scan(1.0f));
    }

    public void SetMemberBoids()
    {
        foreach (UnitObject member in members)
        {
            BoidCohesion cohesion = member.GetComponent<BoidCohesion>();
            BoidSeperation seperation = member.GetComponent<BoidSeperation>();
            foreach (GameObject GO in memberObjects)
            {
                if (GO == member.gameObject)
                {
                    continue;
                }

                cohesion.targets = memberObjects;
                seperation.targets = memberObjects;
            }
            member.transform.parent = null;
        }
    }

    private void Update()
    {
        if(pathfinder.reachedDestination)
        {
            isMoving = false;
        }
    }

    public void SetPlayer(bool value)
    {
        isPlayer = value;
    }

    Vector3 CirclePosition(Vector3 center, float radius, int count)
    {
        float ang = (360 / data.Soldiers.Count) * count;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }

    public void SetTargetEnemy(object unit)
    {
        //Empty
    }

    public void SetTargetLocation(Vector3 point)
    {
        isMoving = true;
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
                    foreach (UnitObject member in members)
                    {
                        member.SetTarget(currentClosestEnemy);
                    }
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
        foreach (UnitObject member in members)
        {
            member.SetTarget(null);
        }
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

    public void TakeHit(WeaponInfo shootingWeapon)
    {
        //Calculate which soldier is hit
        int rand = Random.Range(0, members.Count);

        if(members.Count < rand)
        {
            Debug.Log("Rand was higher than member count - must've died");
            return;
        }
        UnitObject hitTarget = members[rand];

        hitTarget.TakeHit(shootingWeapon);
    }

    public void RemoveMember(UnitObject member)
    {
        members.Remove(member);
        memberObjects.Remove(member.gameObject);

        if(members.Count < 1)
        {
            manager.RemoveUnit(this, isPlayer);
            foreach (SquadObject enemy in enemiesSpotting)
            {
                enemy.SpottingEnemyDestroyed(this);
            }
            Destroy(pathfinder);
        }

        SetMemberBoids();
    }
}
