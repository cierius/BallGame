using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton style game state manager that is very basic. Provides access to the current game state and setting Game State
/// </summary>
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; } = null;

    public enum GameState
    {
        Menu,
        Loading,
        Starting,
        Playing,
        Defeat,
        Leaderboard,
        Exiting
    }

    public static GameState State { get; private set; }

    // Singleton approach, only need one GSM
    private void Awake()
    {
        if(Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    public void SetState(GameState nextState) => State = nextState;
}
