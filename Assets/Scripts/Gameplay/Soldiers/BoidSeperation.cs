using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSeperation : Flee
{
    public float desiredSeperation = 1;
    public List<GameObject> targets;

    public override Steering GetSteering()
    {
        Steering steer = new Steering();
        int count = 0;

        foreach(GameObject other in targets)
        {
            if(other != null)
            {
                float d = (transform.position - other.transform.position).magnitude;

                if(d > 0 && d < desiredSeperation)
                {
                    Vector3 diff = transform.position - other.transform.position;
                    diff.Normalize();
                    diff /= d;
                    steer.linear += diff;
                    count++;
                }
            }
        }

        if(count > 0)
        {
            steer.linear /= (float)count;
        }

        return steer;
    }
}
