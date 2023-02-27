using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour
{
    public SquadObject unit;

    public void Initialize(SquadObject newUnit, List<Transform> objectives)
    {
        unit = newUnit;

        Transform target = objectives[0];
        float distance = 0;

        foreach(Transform objective in objectives)
        {
            float dist = Vector3.Distance(newUnit.transform.position, objective.position);
            if (distance == 0 || dist < distance)
            {
                distance = dist;
                target = objective;
            }
        }

        unit.SetTargetLocation(target.position);
    }
}
