using UnityEditor.Search.Providers;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public UIManager uiManager => GameManager.Instance.UIManager;

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //public void LoadLevel(string sceneName)
    //{
    //    SceneManager.LoadScene(sceneName);       
    //}

    public void LoadScene(int sceneId)
    {
        Debug.Log("Load Scene");

        StartCoroutine(LoadSceneAsync(sceneId));
    }

    public void LoadMainMenu()
    {
        LoadScene(1);
    }



    IEnumerator LoadSceneAsync(int sceneId)
    {
        GameManager.Instance.GameStateManager.SwitchToState(GameState_Loading.Instance);

        Debug.Log("LoadSceneAsync started for scene ID: " + sceneId);


        // Wait one frame to ensure UI is properly initialized
        yield return null;

        SceneManager.sceneLoaded += OnSceneLoaded;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneId);

        // Prevent scene activation until we're ready
        asyncLoad.allowSceneActivation = false;

        float artificialProgress = 0f;
        float minUpdateInterval = 0.005f; // Time between updates in seconds
        float maxUpdateInterval = 0.5f; // Time between updates in seconds
        float minProgressIncrement = 0.005f; // Minimum progress increase per update
        float maxProgressIncrement = 0.05f; // Maximum progress increase per update
        float progressCompletedDelayDuration = 1.0f; // Delay after reaching 100% before completing

        while (!asyncLoad.isDone)
        {
            // Progress goes from 0 to 0.9
            float realProgress = asyncLoad.progress;

            // Gradually increase artificial progress
            artificialProgress = Mathf.MoveTowards(
                artificialProgress,
                realProgress,
                Random.Range(minProgressIncrement, maxProgressIncrement)
            );

            if (realProgress >= 0.9f && artificialProgress >= 0.9f)
            {
                // Set progress to 100% before the hold
                artificialProgress = 1.0f;
                uiManager.loadingUIController.UpdateProgressBar(artificialProgress);

                Debug.Log("Loading completed, holding for display...");

                // Hold at 100% for desired duration
                yield return new WaitForSeconds(progressCompletedDelayDuration);

                Debug.Log("Hold complete, activating scene...");

                // Now allow the scene to activate
                asyncLoad.allowSceneActivation = true;
            }
            else
            {
                // Normalize progress to 0-1 range
                artificialProgress = Mathf.Clamp01(artificialProgress / 0.9f);
            }

            uiManager.loadingUIController.UpdateProgressBar(artificialProgress);

            // Wait for the specified interval before next update
            yield return new WaitForSeconds(Random.Range(minUpdateInterval, maxUpdateInterval));
        }

    }

    //GameManager.Instance.GameStateManager.SwitchToState(GameState_Loading.Instance);

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        Debug.Log("OnSceneLoaded");

        if (scene.name == "Main Menu" || scene.name == "BootLoader")
        {
            // Switch to Menu state
            GameManager.Instance.GameStateManager.SwitchToState(GameState_MainMenu.Instance);
        }
        else if (scene.name == "Level1" || scene.name == "Level2")
        {
            // Switch to Gameplay state
            GameManager.Instance.GameStateManager.SwitchToState(GameState_Gameplay.Instance);

            // Move Player to SpawnPoint
            SpawnPoint spawn = FindAnyObjectByType<SpawnPoint>();
            if (spawn != null)
            {
                GameManager.Instance.PlayerController.transform.position = spawn.transform.position;
                GameManager.Instance.PlayerController.transform.rotation = spawn.transform.rotation;
            }
        }

    }
}
