using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPathHandler : MonoBehaviour
{
    public Transform transformRootObject;

    WayPointNode[] waypointNodes;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if (transformRootObject == null)
            return;

        //Get all Waypoints
        waypointNodes = transformRootObject.GetComponentsInChildren<WayPointNode>();

        //Iterate the list
        foreach (WayPointNode waypoint in waypointNodes)
        {
            foreach (WayPointNode nextWayPoint in waypoint.nextWayPointNode)
            {
                if (nextWayPoint != null)
                    Gizmos.DrawLine(waypoint.transform.position, nextWayPoint.transform.position);
            }
        }
    }
}
