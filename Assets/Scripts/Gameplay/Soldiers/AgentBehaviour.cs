using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBehaviour : MonoBehaviour
{
    public float weight = 1.0f;

    public SquadObject target;
    protected SteeringAgent agent;
    public Vector3 dest;

    public float maxSpeed = 50.0f;
    public float maxAcceleration = 50.0f;
    public float maxRotation = 5.0f;
    public float maxAngularAcceleration = 5.0f;

    public virtual void Start()
    {
        agent = gameObject.GetComponent<SteeringAgent>();
    }

    public virtual void Update()
    {
        agent.SetSteering(GetSteering(), weight);
        transform.rotation = target.transform.rotation;
    }

    public float MapToRange(float rotation)
    {
        rotation %= 360.0f;
        if(Mathf.Abs(rotation) > 180.0f)
        {
            if(rotation < 0)
            {
                rotation += 360.0f;
                return rotation;
            }

            rotation -= 360.0f;
            return rotation;
        }

        return rotation;
    }

    public virtual Steering GetSteering()
    {
        return new Steering();
    }
}
