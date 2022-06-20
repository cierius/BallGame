using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    // Reference to the player position for use in turning onTrigger = false/true based on height
    private Transform playerPos;

    // Basic game state
    public bool hasGameStarted = true;
    public bool shouldMoveLeft = false; // The direction of travel

    [Range(-20f, 20f)] public float left, right;
    private Vector3 leftEnd, rightEnd;
    private Vector3 leftEndGiz, rightEndGiz; // Left and right Gizmos for the paddle movement
    public float variance = 0.05f; // How much wiggle room we have when the paddle gets close to the ends so it doesn't shoot past them 
    public float speed = 5f;
    [SerializeField, Range(0f, 20f)] private float maxSpeed = 10f;

    public bool showGizmos = false;

    // Gizmos for showing where the paddle is going to bounce between
    private void OnDrawGizmos()
    {
        if(showGizmos)
        {
            leftEndGiz = new Vector3(left + transform.position.x, transform.position.y, transform.position.z);
            rightEndGiz = new Vector3(right + transform.position.x, transform.position.y, transform.position.z);

            Gizmos.color = new Color(1f, 1f, 1f, 0.25f);
            Gizmos.DrawCube(leftEndGiz, new Vector3(0.25f, 0.25f, 0.25f));
            Gizmos.DrawCube(rightEndGiz, new Vector3(0.25f, 0.25f, 0.25f));
            Gizmos.DrawLine(leftEndGiz, rightEndGiz);
        }
    }


    private void Awake()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        leftEnd = new Vector3(left + transform.position.x, transform.position.y, transform.position.z);
        rightEnd = new Vector3(right + transform.position.x, transform.position.y, transform.position.z);
    }


    private void FixedUpdate()
    {
        if(hasGameStarted)
        {
            Move();

            // This basically allows the player to jump up through the paddles but then once the player is above the collider is changed to not being a trigger.
            // Also, switches the paddle back to being a trigger if the player falls below the paddle.
            if (playerPos.position.y < transform.position.y)
                GetComponent<Collider>().isTrigger = true;
            else
                GetComponent<Collider>().isTrigger = false;
        }
    }


    // Movement of the paddle function
    private void Move()
    {
        if (shouldMoveLeft)
        {
            // Moving left
            transform.position += new Vector3(-speed, 0f, 0f) * Time.deltaTime;

            if (transform.position.x <= leftEnd.x + variance) shouldMoveLeft = false;
        }
        else
        {
            // Moving Right
            transform.position += new Vector3(speed, 0f, 0f) * Time.deltaTime;

            // The end point on the rightside is where the paddle will gain speed every time until it caps out at a set limit
            if (transform.position.x >= rightEnd.x - variance)
            { 
                shouldMoveLeft = true;
                speed += 1f;
                speed = Mathf.Clamp(speed, 0f, maxSpeed);
            }
        }

        // Clamp the position of the paddle between the left and right ends plus the variance wiggle room
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftEnd.x - variance, rightEnd.x + variance), transform.position.y, transform.position.z);
    }


    // When the player jumps up through the collider make sure to turn off trigger once exited. 
    // This function is slightly redudant as the height of the player is already used to determine if the paddle will be a trigger or not.
    // I figure that having this will safeguard the fact that if the player jumps straight up through, it will be a collider once the player gets even a pixel above the trigger.
    private void OnTriggerExit(Collider coll)
    {
        if(coll.tag == "Player")
        {
            GetComponent<Collider>().isTrigger = false;
        }
    }
}
