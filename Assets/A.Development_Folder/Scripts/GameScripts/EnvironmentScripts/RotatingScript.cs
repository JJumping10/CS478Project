using System;
using UnityEngine;

public class RotatingScript : MonoBehaviour
{
    public bool isCounterClockwise;
    public float rotSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Speed is moving in negative value for counter clockwise or postive for clockwise
        if (isCounterClockwise) {
            if (rotSpeed > 0) rotSpeed = -rotSpeed;
        } else { 
            rotSpeed = Math.Abs(rotSpeed);
        }
        transform.Rotate(0,0,rotSpeed);
    }
}
