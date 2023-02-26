using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering
{
    public float angular; //Rotation 0-360
    public Vector3 linear; //Instantaneous Velocity

    public Steering()
    {
        angular = 0.0f;
        linear = new Vector3(0,0,0);
    }
}
