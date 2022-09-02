using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCamera : MonoBehaviour
{
    [SerializeField] Camera cameraDistance;
    [SerializeField] float cameraMovementSpeed = 1;
    // this things position (camera) should be the same as the car's position

    private float startingSize;
    private float endSize;
    private float t = 0;
    public bool cameraMove = false;
    public float cameraStartMoveX;
    public float cameraStartMoveY;

    private void Awake()
    {
        startingSize = cameraDistance.orthographicSize;
        endSize = startingSize * 4;
    }

    private void Update()
    {
        if (cameraMove)
        {
            t += Time.deltaTime / cameraMovementSpeed;
            cameraDistance.orthographicSize = Mathf.SmoothStep(startingSize, endSize, t);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Car")
        {
            cameraMove = true;
            cameraStartMoveX = other.transform.position.x;
            cameraStartMoveY = other.transform.position.y;
            //Debug.Log(other.transform.position);
        }
    }
}