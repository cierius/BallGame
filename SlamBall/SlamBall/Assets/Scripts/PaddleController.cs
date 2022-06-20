using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    private Transform playerPos;

    public bool hasGameStarted = true;
    public bool shouldMoveLeft = false; // The direction of travel

    public Vector2 leftEnd, rightEnd; // Left point where the paddle goes, and right point
    public float variance = 0.05f; // How much wiggle room we have when the paddle gets close to the ends so it doesn't shoot past them 
    public float speed = 5f;

    public bool showGizmos = false;

    // Gizmos for showing where the paddle is going to bounce between
    private void OnDrawGizmos()
    {
        if(showGizmos)
        {
            leftEnd = new Vector2(leftEnd.x, transform.position.y);
            rightEnd = new Vector2(rightEnd.x, transform.position.y);

            Gizmos.color = new Color(1f, 1f, 1f, 0.25f);
            Gizmos.DrawCube(leftEnd, new Vector3(0.25f, 0.25f, 0.25f));
            Gizmos.DrawCube(rightEnd, new Vector3(0.25f, 0.25f, 0.25f));
            Gizmos.DrawLine(leftEnd, rightEnd);
        }
    }


    private void Awake()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }


    private void FixedUpdate()
    {
        if(hasGameStarted)
        {
            Move();

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

            if (transform.position.x >= rightEnd.x - variance)
            { 
                shouldMoveLeft = true;
                speed += 1f;
            }
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftEnd.x - variance, rightEnd.x + variance), transform.position.y, 0f);
    }


    private void OnTriggerExit(Collider coll)
    {
        if(coll.tag == "Player")
        {
            GetComponent<Collider>().isTrigger = false;
        }
    }
}
