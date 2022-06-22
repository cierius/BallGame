using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Singleton style level manager.
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; } = null;

    private Slider progressBar;

    // Singleton approach, only need one LevelManager
    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// Resets active scene
    /// </summary>
    public void OnResetLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    // Immediately loads the menu
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    /// <summary>
    /// When called load the loading screen and start async loading the level asked for.
    /// </summary>
    /// <param name="index"></param>
    public IEnumerator LoadLevel(int index)
    {
        // Switch scene to the loading screen
        SceneManager.LoadScene("Scenes/LoadingScreen", LoadSceneMode.Single);

        //if(SceneManager.GetActiveScene().name == "LoadingScreen")
            //progressBar = GameObject.FindWithTag("LoadingBar").GetComponent<Slider>();

        while(SceneManager.GetActiveScene().name != "LoadingScreen")
        {
            yield return new WaitForEndOfFrame(); 
        }

        // Start the async loading
        StartCoroutine(LoadAsync(index));
        yield return null;
    }

    // Loads a scene async while proving progress updates every frame
    private IEnumerator LoadAsync(int index)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);

        // Switch to the scene immediately once it's done loading
        asyncLoad.allowSceneActivation = false;
        

        

        while(!asyncLoad.isDone)
        {
            // Update the progress bar on the screen while it loads
            //progressBar.value = asyncLoad.progress;
            yield return new WaitForSeconds(2);
        }

        yield return new WaitForSeconds(2);
    }

    public void LoadLeaderboard()
    {

    }

    /// <summary>
    /// When called immediately quits the game.
    /// </summary>
    public void ExitGame()
    {
        // When running on Desktop builds
        if (!Application.isMobilePlatform && !Application.isEditor)
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        else
            Application.Quit();
    }
}
