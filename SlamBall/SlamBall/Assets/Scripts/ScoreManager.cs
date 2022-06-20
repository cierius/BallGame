using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // Reference to player so we can get the current height
    private GameObject player;

    // Score / Highest Jump variables
    private int score = 0;
    private float maxJumpHeight = 0.0f;

    // UI text for the score and highest jump
    public Text scoreText, jumpHeightText;


    // On start of game, update the ui to be zeros / whatever value is needed
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        UpdateUI();
    }

    private void FixedUpdate()
    {
        GetJumpHeight(player.transform.position.y);
    }

    /// <summary>
    /// Function that updates the UI on call.
    /// </summary>
    private void UpdateUI()
    {
        scoreText.text = $"{score}";
        jumpHeightText.text = maxJumpHeight.ToString("F1");
    }

    /// <summary>
    /// Function that is used to add score to the ScoreManager, then updates the UI.
    /// </summary>
    /// <param name="amount"></param>
    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    // Getter for the score
    public int GetScore() => score;
    



    /// <summary>
    /// Gets the current height of the player and then checks it against the current highest jump. maxJumpHeight is the higher of the values.
    /// </summary>
    public void GetJumpHeight(float height)
    {
        maxJumpHeight = Mathf.Max(maxJumpHeight, height);
        UpdateUI();
    }
}
