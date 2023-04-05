using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class SectorCamera : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private int MapX;
    [SerializeField] private int MapY;
    [SerializeField] private int maxZoom;
    [SerializeField] private int minZoom;

    private Vector3 positionChange;
    private float rotationChange;
    private bool isMoving = false;
    private bool isRotating = false;
    private Vector3 pressedPosition;
    private float rotationYValue;
    private float screenX;
    private float zoomHeight;

    private void Update()
    {
        if (transform.position.x <= MapX && transform.position.x >= -MapX && transform.position.z <= MapY && transform.position.z >= -MapY)
        {
            DoMovement();
        }
        else
        {
            ReturnMovement();
        }


        if (Input.GetMouseButtonDown(2))
        {
            StartRotation();
        }
        if (Input.GetMouseButtonUp(2))
        {
            StopRotation();
        }

        DoRotation();
        DoZoom();
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

    private void DoMovement()
    {
        positionChange = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            positionChange += transform.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            positionChange -= transform.forward;
        }

        if (Input.GetKey(KeyCode.D))
        {
            positionChange += transform.right;
        }

        if (Input.GetKey(KeyCode.A))
        {
            positionChange -= transform.right;
        }

        if (positionChange == Vector3.zero)
        {
            isMoving = false;
            return;
        }

        isMoving = true;

        transform.localPosition += positionChange * moveSpeed;
    }

    private void ReturnMovement()
    {
        if (transform.position.x > MapX || transform.position.x < -MapX)
        {
            if ((MapX - transform.localPosition.x) > -(-MapX - transform.localPosition.x))
            {
                transform.localPosition = new Vector3(-MapX, 0, transform.localPosition.z);
            }
            if ((MapX - transform.localPosition.x) < -(-MapX - transform.localPosition.x))
            {
                transform.localPosition = new Vector3(MapX, 0, transform.localPosition.z);
            }
        }

        if (transform.position.z > MapY || transform.position.z < -MapY)
        {
            if ((MapY - transform.localPosition.z) > -(-MapY - transform.localPosition.z))
            {
                transform.localPosition = new Vector3(transform.localPosition.x, 0, -MapY);
            }
            if ((MapY - transform.localPosition.z) < -(-MapY - transform.localPosition.z))
            {
                transform.localPosition = new Vector3(transform.localPosition.x, 0, MapY);
            }
        }
    }

    private void DoRotation()
    {
        if (isRotating == false)
        {
            KeyboardRotation();
            return;
        }

        screenX = Input.mousePosition.x - pressedPosition.x;
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

        if (rotationYValue != 0)
        {
            //Rotate our position component on the Y
            float rotValue = transform.rotation.eulerAngles.y + ((rotationYValue * rotateSpeed));
            Quaternion newRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, rotValue, 0);
            newRotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
            transform.localRotation = newRotation;
        }
    }

    private void KeyboardRotation()
    {
        rotationChange = 0;

        if (Input.GetKey(KeyCode.Q))
        {
            rotationChange = -1;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotationChange = 1;

        }

        float rotationValue = transform.rotation.eulerAngles.y + (rotationChange * rotateSpeed);

        Quaternion newRotation = Quaternion.Euler(0, rotationValue, 0);
        newRotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        transform.localRotation = newRotation;
    }

    private void DoZoom()
    {
        float wheelValue = Input.GetAxis("Mouse ScrollWheel");
        int zoomY = 0;
        Vector3 zoom = Vector3.zero;

        if (wheelValue != 0)
        {
            if (wheelValue > 0)
            {
                zoom = -transform.forward;
                zoomY = -1;
            }
            if (wheelValue < 0)
            {
                zoom = transform.forward;
                zoomY = 1;
            }
            CheckMouseTarget();
        }

        Vector3 position = transform.localPosition;

        if (zoom != Vector3.zero)
        {
            if (camera.transform.localPosition.y > maxZoom || camera.transform.localPosition.y < minZoom)
            {
                Vector3 newZoom = -(zoom * zoomSpeed);
                Vector3 newZoomY = new Vector3(0, zoomY * zoomSpeed, 0);
                transform.localPosition += newZoom;
                camera.transform.localPosition += newZoomY;
                zoomHeight = newZoomY.y;
            }
        }
        if (camera.transform.localPosition.y <= maxZoom)
        {
            camera.transform.localPosition = new Vector3(0, maxZoom + 1, 0);
            transform.localPosition = position;
        }
        if (camera.transform.localPosition.y >= minZoom)
        {
            camera.transform.localPosition = new Vector3(0, minZoom - 1, 0);
            transform.localPosition = position;
        }
    }

    private void CheckMouseTarget()
    {

    }
}
