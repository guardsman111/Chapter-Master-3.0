using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringAgent : MonoBehaviour
{
    public float maxSpeed = 10.0f;
    public float trueMaxSpeed;
    public float maxAcceleration = 30.0f;

    public float orientation;
    public float rotation;
    public Vector3 velocity;
    protected Steering steering;

    public float maxRotation = 45.0f;
    public float maxAngularAcceleration = 45.0f;

    private void Start()
    {
        velocity = Vector3.zero;
        steering = new Steering();
        trueMaxSpeed = maxSpeed;
    }

    public void SetSteering(Steering steer, float weight)
    {
        steering.linear += (weight * steer.linear);
    }

    //Change the trasnform based off of the last frame
    protected virtual void Update()
    {
        Vector3 displacement = velocity * Time.deltaTime;
        displacement.y = 0;

        transform.Translate(displacement, Space.World);
    }

    //Update movement for the next frame
    protected virtual void LateUpdate()
    {
        velocity += steering.linear * Time.deltaTime;

        if(velocity.magnitude > maxSpeed)
        {
            velocity.Normalize();
            velocity = velocity * maxSpeed;
        }

        if(steering.linear.magnitude == 0.0f)
        {
            velocity = Vector3.zero;
        }

        steering = new Steering();
    }

    public void speedReset()
    {
        maxSpeed = trueMaxSpeed;
    }
}
