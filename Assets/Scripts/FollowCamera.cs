using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // this things position (camera) should be the same as the car's position
    [SerializeField] GameObject thingToFollow;
    // when finish line is crossed camera focus moves here
    [SerializeField] GameObject thingToFocus;

    public DistanceCamera distanceCamera;
    private float transformX;
    private float transformY;
    private float t = 0;

    // LateUpdate() is called after Update() and works better with follow camera
	// as it tracks movement that might have happened in Update()
	void LateUpdate()
    {
        if (distanceCamera.cameraMove == false)
        {
            transform.position = thingToFollow.transform.position + new Vector3(0, 0, -10);
        }
        else
        {
            t += Time.deltaTime / 5;
            transformX = Mathf.SmoothStep(distanceCamera.cameraStartMoveX, thingToFocus.transform.position.x, t);
            transformY = Mathf.SmoothStep(distanceCamera.cameraStartMoveY, thingToFocus.transform.position.y -15, t);

            transform.position = new Vector3 (transformX, transformY, -10);
        }
    }
}
