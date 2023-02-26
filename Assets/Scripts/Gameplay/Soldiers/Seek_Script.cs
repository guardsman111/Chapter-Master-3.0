using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Seek_Script : MonoBehaviour
{
    Base_Behaviour bb;
    GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        bb = gameObject.GetComponent<Base_Behaviour>();


        if (bb.seekScript == null)
        {
            bb.seekScript = gameObject.AddComponent<Seek>();
            bb.seekScript.weight = 0.7f;
            bb.seekScript.enabled = true;
/*

            bb.boidcoh = gameObject.AddComponent<BoidCohesion>();
            bb.boidcoh.targets = bb.target.GetComponent<squad_parent_script>().children;
            bb.boidcoh.weight = 0.4f;
            bb.boidcoh.enabled = true;

            bb.boidsep = gameObject.AddComponent<BoidSeparation>();
            bb.boidsep.targets = bb.target.GetComponent<squad_parent_script>().children;
            bb.boidsep.weight = 70.0f;
            bb.boidsep.enabled = true;
*/
        }

    }


    /*private void OnDrawGizmos()
    {
        UnityEditor.Handles.Label(transform.position + Vector3.up * 3, "Seek");
    }*/

}
