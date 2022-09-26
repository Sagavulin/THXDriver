using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpeed : MonoBehaviour
{
    [SerializeField] Rigidbody2D TheCar;
    private float TheSpeed;

    void Start()
    {
        AkSoundEngine.PostEvent("Play_carEngine", gameObject);
    }

    void Update()
    {
        TheSpeed = TheCar.velocity.magnitude;
        //Debug.Log(TheSpeed);
        AkSoundEngine.SetRTPCValue("CarSpeed", TheSpeed, gameObject);
    }
}
