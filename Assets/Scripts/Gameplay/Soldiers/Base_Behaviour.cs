using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Base_Behaviour : MonoBehaviour
{
    //gps is our general pathfinding script
    //public general_pathfinding gps;

    //intelligent movement scripts
    public SteeringAgent agentScript;

    public Seek seekScript;
    public Flee fleeScript;
    public BoidCohesion boidCohesion;
    public BoidSeperation boidSeperation;

    public float maxSpeed;

    public SquadObject target;
    public UnitFSM state;

    public enum UnitFSM //states
    {
        Idle,
        Seek
    }

    // Start is called before the first frame update
    void Start()
    {
        agentScript = gameObject.AddComponent<SteeringAgent>(); //add agent
        agentScript.maxSpeed = maxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(!target.isMoving)
        {
            if (Vector3.Distance(transform.position, target.transform.position) < 5)
            {
                changeState(UnitFSM.Idle);
                return;
            }
        }

        changeState(UnitFSM.Seek);
    }

    public void SetTarget(SquadObject value)
    {
        seekScript.target = value;
        fleeScript.target = value;
        boidCohesion.target = value;
        boidSeperation.target = value;
        target = value;
    }

    public void changeState(UnitFSM new_state)
    {
        state = new_state;

        switch (new_state)
        {
            case UnitFSM.Idle:
                seekScript.weight = 0f;

                break;
            case UnitFSM.Seek:
                seekScript.weight = 0.7f;

                break;
        }
    }
}
