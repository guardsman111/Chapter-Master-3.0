using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NameTag : MonoBehaviour
{
    [SerializeField] private TextMeshPro label;

    public Camera Camera;

    private Vector3 targetPoint;
    private Quaternion targetRotation;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Camera == null)
        {
            return;
        }

        targetPoint = new Vector3(Camera.transform.position.x, transform.position.y, Camera.transform.position.z) - transform.position;
        targetRotation = Quaternion.LookRotation(-targetPoint, Vector3.up);
        transform.localRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);
    }

    public virtual void Initialize(string name, Camera cam)
    {
        label.text = name;
        Camera = cam;
    }
}
