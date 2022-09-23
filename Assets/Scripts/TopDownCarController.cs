using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCarController : MonoBehaviour
{
    [SerializeField] float driftFactor = 0.95f;
    [SerializeField] float accelerationFactor = 30.0f;
    [SerializeField] float turnFactor = 3.5f;
    [SerializeField] float maxSpeed = 20.0f;

    private float accelerationInput = 0;
    private float steeringInput = 0;
    private float rotationAngle = 0;
    private float velocityVsUp = 0;
    private Rigidbody2D carRigidBody;

    void Awake()
    {
        carRigidBody = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate is used because use of Rigidbody; keeps better sync with physics engine
	void FixedUpdate()
    {
        ApplyEngineForce();

        KillOrthogonalVelocity();

        ApplySteeringForce();
    }

    void ApplyEngineForce()
    {
        velocityVsUp = Vector2.Dot(transform.up, carRigidBody.velocity);

        if (velocityVsUp > maxSpeed && accelerationInput > 0)
            return;

        if (velocityVsUp < -maxSpeed * 0.5 && accelerationInput < 0)
            return;

        if (carRigidBody.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
            return;

        if (accelerationInput == 0)
            carRigidBody.drag = Mathf.Lerp(carRigidBody.drag, 3.0f, Time.fixedDeltaTime * 3);
        else carRigidBody.drag = 0;
        
        Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;

        carRigidBody.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplySteeringForce()
    {
        // limit the car's ability to turn when moving slowly
        float minSpeedBeforeAllowTurningFactor = (carRigidBody.velocity.magnitude / 8);
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);
        
        rotationAngle -= steeringInput * turnFactor * minSpeedBeforeAllowTurningFactor;

        carRigidBody.MoveRotation(rotationAngle);
    }

    void KillOrthogonalVelocity()
    {
        // Get forward and right velocity of the car
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidBody.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidBody.velocity, transform.right);

        // Kill the orthogonal velocity (side velocity) based on how much the car should drift
        carRigidBody.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    float GetLateralVelocity()
    {
        // Returns how fast the car is moving
        return Vector2.Dot(transform.right, carRigidBody.velocity);
    }
    
	public bool IsTireScreeching (out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = GetLateralVelocity();
        isBraking = false;

        if (accelerationInput < 0 && velocityVsUp > 0)
        {
            isBraking = true;
            return true;
        }

        if (Mathf.Abs(GetLateralVelocity()) > 4.0f)
            return true;

        return false;
    }

    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }

    public float GetVelocityMagnitude()
    {
        return carRigidBody.velocity.magnitude;
    }

    public float GetVelocityVsUp()
    {
        return velocityVsUp;
    }
}