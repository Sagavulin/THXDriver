using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CarAIHandler : MonoBehaviour
{
    public enum AIMode { followPlayer, followWayPoints} ;

    [Header("AI Settings")]
    public AIMode aiMode;
    public float maxSpeed = 16;


    // Local variables
    Vector3 targetPosition = Vector3.zero; // for following wayPoints
    Transform targetTransform = null; // for following player

    // Waypoints
    WayPointNode currentWayPoint = null;
    WayPointNode[] allWayPoints;

    // Components
    TopDownCarController topDownCarController;
    
    void Awake()
    {
        topDownCarController = GetComponent<TopDownCarController>();
        allWayPoints = FindObjectsOfType<WayPointNode>();
    }
    
    void FixedUpdate()
    {
        Vector2 inputVector = Vector2.zero;
        
        switch (aiMode)
        {
            case AIMode.followPlayer:
                FollowPlayer();
                break;

            case AIMode.followWayPoints:
                FollowWayPoints();
                break;
        }
        
        inputVector.x = TurnTowardTarget(); // steering
        inputVector.y = ApplyThrottleOrBreak(inputVector.x); // acceleration

        // Send the input to the car controller
        topDownCarController.SetInputVector(inputVector);        
    }

    void FollowPlayer()
    {
        if (targetTransform == null){
            targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (targetTransform != null){
            targetPosition = targetTransform.position;
        }
    }

    void FollowWayPoints()
    {
        // Pick the closest waypoint if we don't have waypoint set
        if (currentWayPoint == null)
        {
            currentWayPoint = FindClosestWayPoint();
        }

        if (currentWayPoint != null)
        {
            targetPosition = currentWayPoint.transform.position;

            // Store how close we are to the target
            float distanceToWayPoint = (targetPosition -transform.position).magnitude;
            
            // Check if we are close enough to consider that we have reached the waypoint
            if (distanceToWayPoint <= currentWayPoint.mindistanceToReachWayPoint)
            {
                if (currentWayPoint.maxSpeed > 0)
                    maxSpeed = currentWayPoint.maxSpeed;
                else maxSpeed = 20;

                // if we are close enough then follow to the next waypoint, if there multiple waypoints then pick one at random
                currentWayPoint = currentWayPoint.nextWayPointNode[Random.Range(0, currentWayPoint.nextWayPointNode.Length)];
            }
        }
    }

    WayPointNode FindClosestWayPoint()
    {
        return allWayPoints
        .OrderBy(t => Vector3.Distance(transform.position, t.transform.position))
        .FirstOrDefault();
    }

    float TurnTowardTarget()
    {
        Vector2 vectorToTarget = targetPosition - transform.position;
        vectorToTarget.Normalize();

        // Calculate an angle towards the target
        float angleToTarget = Vector2.SignedAngle(transform.up, vectorToTarget);
        angleToTarget *= -1; // reverse the angle

        // car should turn as much as possible when the angle is greater than 45degrees and smooth out if the angle is smaller
        float steerAmount = angleToTarget / 45.0f;

        // Clamp steering to between -1 and 1
        steerAmount = Mathf.Clamp(steerAmount, -1.0f, 1.0f);

        return steerAmount;
    }
    
    float ApplyThrottleOrBreak(float inputX)
    {
        // If car is going too fast then do not accelerate further
        if (topDownCarController.GetVelocityMagnitude() > maxSpeed)
            return 0;

        // Apply throttle forward based on how much the car wants to turn.
        // If it's a sharp turn this cause the car to apply less speed forward.
        return 1.05f - Mathf.Abs(inputX) / 1.0f;
    }
}
