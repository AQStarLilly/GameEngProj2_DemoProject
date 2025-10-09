using UnityEditor.Search.Providers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level1" || scene.name == "Level2")
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
        else if (scene.name == "Main Menu")
        {
            // Switch to Menu state
            GameManager.Instance.GameStateManager.SwitchToState(GameState_MainMenu.Instance);
        }
    }
    
}
