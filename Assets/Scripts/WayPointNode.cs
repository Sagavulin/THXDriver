using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointNode : MonoBehaviour
{
    // What is the max speed allowed when passing this waypoint
    [Header("Speed set once we reach the waypoint")]
    public float maxSpeed = 0;
    
    [Header("This is the waypoint we are going towards, not yet reached")]
    public float mindistanceToReachWayPoint = 5;

    public WayPointNode[] nextWayPointNode;

}
