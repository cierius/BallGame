using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    // References
    private Rigidbody rb;

    // Gyro variables
    private Gyroscope gyro;
    private Vector3 rot; // gyro's rotational
    private bool gyroActive = false;

    // Debug Gyro Variables
    public Text gyroEnabled, gyroRot, gyroAttitude;

    // Ball movement variables
    public float slamAccel = 2.5f; // The rate at which the ball accelerates downward when being "slammed"
    public float horizontalSpeed = 1f;
    public bool isMoveable = true;
    public bool isTraversable = true;

    //Score
    public int score;
    public float maxHeight;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // Try to enable the gyro of the device
        if(EnableGyro())
            print($"Gyro {gyroActive}");
    }

    void Update()
    {
        // Slam the ball when a touch is detected on the screen
        if(CheckTouch())
            rb.AddForce(new Vector3(0f, -slamAccel, 0f), ForceMode.Force);
        
        // Move the ball left / right based on the phone tilting left / right
        if(isTraversable)
            BallTraverse();

        // PC Debugging
        if(Input.GetMouseButton(0))
        {
            rb.AddForce(new Vector3(0f, -slamAccel, 0f), ForceMode.Force);
        }

        // Reset the ball via keyboard or when the ball falls below a certain point, for use while developing
        if(Input.GetKeyDown(KeyCode.Space) || transform.position.y <= -5f)
        {
            ResetBall();
        }
    }


    private void ResetBall()
    {
        rb.velocity = Vector3.zero;
        transform.position = Vector3.zero;
    }

    /// <summary>
    /// Function for managing touches, returns true if there was a touch.
    /// </summary>
    private bool CheckTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                print("Touch Began");
                return true;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                print("Touch Ended");
                return false;
            }
        }

        return false;
    }


    /// <summary>
    /// Function that manages the gyroscope of the device in order to translate the ball left / right.
    /// </summary>
    private void BallTraverse()
    {
        if (gyroActive)
        {
            rot = gyro.userAcceleration;

            gyroEnabled.text = $"Gyro enabled: {gyroActive}";
            gyroRot.text = $"Gyro rot: {rot}";
            gyroAttitude.text = $"Gyro Att: {gyro.attitude}";

            transform.position += new Vector3(-rot.z * horizontalSpeed, 0f, 0f) * Time.deltaTime;
        }
    }


    /// <summary>
    /// Function thats tries to enable the device's gyroscope, does nothing if the device doesn't support gyro.
    /// </summary>
    /// <returns></returns>
    private bool EnableGyro()
    {
        if (gyroActive)
            return true;

        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            gyroActive = gyro.enabled;
            return true;
        }
        else
            return false;
    }


    private void OnCollisionEnter(Collision coll)
    {
        if(coll.transform.tag == "Town")
        {
            ResetBall();
        }
        else if(coll.transform.tag == "Paddle")
        {
            score += 1;
        }
    }


}
