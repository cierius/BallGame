using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This scripts main purpose it to manage switching between levels
/// </summary>
public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// Resets active scene
    /// </summary>
    public void OnResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
