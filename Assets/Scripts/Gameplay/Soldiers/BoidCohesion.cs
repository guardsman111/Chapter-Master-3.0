using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidCohesion : AgentBehaviour
{
    public float neightbourDist = 2.0f;
    public List<GameObject> targets;

    public override Steering GetSteering()
    {
        Steering steer = new Steering();
        int count = 0;

        foreach (GameObject other in targets)
        {
            if (other != null)
            {
                float d = (transform.position - other.transform.position).magnitude;

                if (d > 0 && d < neightbourDist)
                {
                    steer.linear += other.transform.position;
                    count++;
                }
            }
        }

        if (count > 0)
        {
            steer.linear /= (float)count;
            steer.linear = steer.linear - transform.position;
        }

        return steer;
    }
}
