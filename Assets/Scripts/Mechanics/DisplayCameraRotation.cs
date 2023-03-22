using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCameraRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float mouseRotationMultiplier;
    [SerializeField] private int maxXRotation;
    [SerializeField] private int minXRotation;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float maxZoom;
    [SerializeField] private float minZoom;
    [SerializeField] private Camera cam;

    private bool isRotating = false;

    private Vector3 pressedPosition;
    private float rotationXValue;
    private float rotationYValue;

    private float screenX;
    private float screenY;

    private int zoom = 0;

    private float wheelValue;

    // Update is called once per frame
    void Update()
    {
        DoRotationCheck();
        DoZoomCheck();
    }

    private void DoRotationCheck()
    {
        if (Input.GetMouseButtonDown(2))
        {
            StartRotation();
        }
        if (Input.GetMouseButtonUp(2))
        {
            StopRotation();
        }

        DoRotation();
    }

    private void DoZoomCheck()
    {
        wheelValue = Input.GetAxis("Mouse ScrollWheel");

        if (wheelValue != 0)
        {
            if (wheelValue > 0)
            {
                zoom = -1;
            }
            if (wheelValue < 0)
            {
                zoom = 1;
            }
        }
        else
        {
            zoom = 0;
        }

        if (zoom != 0)
        {
            if (cam.transform.localPosition.z > minZoom || cam.transform.localPosition.z < maxZoom)
            {
                Vector3 newZoom = new Vector3(0, 0, -(zoom * zoomSpeed));
                cam.transform.localPosition += newZoom;
            }
        }
        if (cam.transform.localPosition.z <= minZoom)
        {
            cam.transform.localPosition = new Vector3(0, cam.transform.localPosition.y, (minZoom));
        }
        if (cam.transform.localPosition.z >= maxZoom)
        {
            cam.transform.localPosition = new Vector3(0, cam.transform.localPosition.y, (maxZoom));
        }
    }

    private void StartRotation()
    {
        isRotating = true;
        pressedPosition = Input.mousePosition;
    }

    private void StopRotation()
    {
        isRotating = false;
    }

    private void DoRotation()
    {
        if (isRotating == true)
        {
            screenX = Input.mousePosition.x - pressedPosition.x;
            screenY = Input.mousePosition.y - pressedPosition.y;
            float screenXPos = 0;

            if (screenX > 6)
            {
                rotationYValue = 1;
                screenXPos = screenX;
                pressedPosition = Input.mousePosition;
            }
            else if (screenX < -6)
            {
                rotationYValue = -1;
                screenXPos = -screenX;
                pressedPosition = Input.mousePosition;
            }
            else
            {
                rotationYValue = 0;
            }
            if (screenY > 3)
            {
                rotationXValue = -1 + ((100 - screenY) / 100);
                pressedPosition = Input.mousePosition;
            }
            else if (screenY < -3)
            {
                rotationXValue = 1 - ((100 - -screenY) / 100);
                pressedPosition = Input.mousePosition;
            }
            else
            {
                rotationXValue = 0;
            }

            if (rotationXValue != 0)
            {
                //Rotate this on the X
                Quaternion newRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x + (rotationXValue * (rotationSpeed)), transform.localRotation.eulerAngles.y, 0);
                newRotation = Quaternion.RotateTowards(transform.localRotation, newRotation, rotationSpeed * Time.deltaTime);
                transform.localRotation = newRotation;

                float x = transform.localRotation.eulerAngles.x;

                if (x > maxXRotation && x < maxXRotation + 50)
                {
                    x = maxXRotation;
                }
                if (x < minXRotation || x >= maxXRotation + 50)
                {
                    x = minXRotation;
                }

                transform.localEulerAngles = new Vector3(x, transform.localRotation.eulerAngles.y, 0);
            }

            if (rotationYValue != 0)
            {
                //Rotate our position component on the Y
                float rotValue = transform.rotation.eulerAngles.y + ((rotationYValue * rotationSpeed));
                RotateRig(rotValue, mouseRotationMultiplier * ((screenXPos / (Screen.width / 2))) * 10);
            }
        }
    }

    public void RotateRig(float newValue, float multiplier = 1)
    {
        Quaternion newRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, newValue, 0);
        newRotation = Quaternion.RotateTowards(transform.rotation, newRotation, (rotationSpeed * Time.deltaTime) * multiplier);
        transform.localRotation = newRotation;
    }
}
