using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BallController : MonoBehaviour
{
    // References
    private ScoreManager score;
    private Rigidbody rb;

    public Text jumpText;

    // Gyro variables
    private Gyroscope gyro;
    private Vector3 rot; // gyro's rotational
    private bool gyroActive = false;

    // Debug Gyro Variables
    public Text gyroEnabled, gyroRot, gyroAttitude;

    // Ball movement variables
    public float jumpForce = 5.0f;
    private bool jumpOnCooldown = false;
    public int jumpsRemaining = 5;
    public float horizontalSpeed = 1f;
    public bool isMoveable = true;
    public bool isTraversable = true;

    private Vector3 origin;

    private void Awake()
    {
        Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;

        rb = GetComponent<Rigidbody>();
        score = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

        jumpText.text = $"{jumpsRemaining}";
    }

    private void Start()
    {
        // Try to enable the gyro of the device
        if(EnableGyro())
            print($"Gyro {gyroActive}");

        origin = transform.position;
    }

    void Update()
    {       
        // Move the ball left / right based on the phone tilting left / right
        if(isTraversable)
            BallTraverse();

        // PC Debugging
        if(Input.GetMouseButton(0) || CheckTouch())
        {
            //rb.AddForce(new Vector3(0f, -slamAccel, 0f), ForceMode.Force);
            StartCoroutine(Jump());
        }

        // Reset the ball via keyboard or when the ball falls below a certain point, for use while developing
        if(Input.GetKeyDown(KeyCode.Space) || transform.position.y <= -5f)
        {
            ResetBall();
        }
    }

    /// <summary>
    /// Resets the ball to its origin position and zeros its velocity
    /// </summary>
    private void ResetBall()
    {
        rb.velocity = Vector3.zero;
        transform.position = origin;
        jumpsRemaining = 5;
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
            //print(gyro.gravity);
            //rot = gyro.userAcceleration;
            rot = gyro.gravity;

            gyroEnabled.text = $"Gyro enabled: {gyroActive}";
            gyroRot.text = $"Gyro rot: {rot}";
            gyroAttitude.text = $"Gyro Att: {gyro.attitude}";

            transform.position += new Vector3(rot.x * horizontalSpeed, 0f, 0f) * Time.deltaTime;
        }
    }


    private IEnumerator Jump()
    {
        if (!jumpOnCooldown && jumpsRemaining > 0)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.VelocityChange);

            jumpsRemaining--;
            jumpText.text = $"{jumpsRemaining}";
            jumpOnCooldown = true;

            yield return new WaitForSeconds(1f);
            
            jumpOnCooldown = false;
        }

        yield return null;
    }

    /// <summary>
    /// Function thats tries to enable the device's gyroscope, returns false if the device doesn't support gyro.
    /// </summary>
    /// <returns></returns>
    private bool EnableGyro()
    {
        if (gyroActive)
            return true;

        // If the system supports gyro then assign a reference to it, enable it, and then return true
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


    /// <summary>
    /// Called when colliding with non-trigger colliders
    /// </summary>
    /// <param name="coll"></param>
    private void OnCollisionEnter(Collision coll)
    {
        if(coll.transform.tag == "Town")
        {
            // When the ball touches the ground below the first paddle
            ResetBall();
        }
        else if(coll.transform.tag == "Paddle")
        {
            if(!coll.collider.isTrigger)
            {
                score.AddScore(1);
                // give player a jump every 5 score
                if (score.GetScore() % 5 == 0 && score.GetScore() != 0)
                {
                    jumpsRemaining++;
                    jumpText.text = $"{jumpsRemaining}";
                }

                rb.AddForce(new Vector3(0f, 2f, 0f), ForceMode.VelocityChange);
            }
                
        }
    }


}
