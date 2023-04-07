using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NameTag : MonoBehaviour
{
    [SerializeField] private TextMeshPro label;

    public Camera Camera;

    [SerializeField] private float multiplier;

    private Vector3 targetPoint;
    private Quaternion targetRotation;

    // Update is called once per frame
    public void RotateTag()
    {
        if(Camera == null)
        {
            return;
        }

        Vector3 currentPoint = new Vector3(transform.position.x, 0, transform.position.z);
        targetPoint = new Vector3(Camera.transform.position.x, 0, Camera.transform.position.z) - currentPoint;
        targetRotation = Quaternion.LookRotation(-targetPoint, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * multiplier);
    }

    public virtual void Initialize(string name, Camera cam)
    {
        label.text = name;
        Camera = cam;
    }
}
