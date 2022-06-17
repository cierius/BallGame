using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    private Rigidbody rb;

    public float slamAccel = 2.5f; // The rate at which the ball accelerates downward when being "slammed"

    public Text gyroText;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        // Currently left mouse button is the slammer, will be touch based in the future
        if(Input.GetMouseButton(0))
        {
            rb.AddForce(new Vector3(0f, -slamAccel, 0f), ForceMode.Acceleration);
        }

        // Reset the ball, use while developing
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = Vector3.zero;
            transform.position = Vector3.zero;
        }

        //gyroText.text = "" + Input.gyro.attitude;
    }
}
