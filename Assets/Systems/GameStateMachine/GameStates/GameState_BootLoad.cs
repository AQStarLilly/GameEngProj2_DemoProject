using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameState_BootLoad : IState
{
    GameManager gameManager => GameManager.Instance;
    GameStateManager gameStateManager => GameManager.Instance.GameStateManager;

    PlayerController playerController => GameManager.Instance.PlayerController;
    UIManager UIManager => GameManager.Instance.UIManager;
    #region Singleton Instance

    private static readonly GameState_BootLoad instance = new GameState_BootLoad();

    public static GameState_BootLoad Instance = instance;

    #endregion


    public void EnterState()
    {
        Time.timeScale = 0f;
        Cursor.visible = false;

        //Detect current scene type and set gamestate accordingly

        //if bootloader is the only active scene, redirect to main menu
        if (SceneManager.sceneCount == 1 && SceneManager.GetActiveScene().name == "BootLoader")
        {
            Debug.Log("Gamesate_Bootload");
            GameManager.Instance.LevelManager.LoadMainMenu();
            return;
        }
        //if bootloader is initialized while in the main menu scene
        else if(SceneManager.sceneCount > 1 && SceneManager.GetActiveScene().name == "Main Menu")
        {
            gameManager.GameStateManager.SwitchToState(GameState_MainMenu.Instance);
            return;
        }
        //if all the above fails the assumption is that we are in a gameplay scene
        else
        {
            gameManager.GameStateManager.SwitchToState(GameState_Gameplay.Instance);
            return;
        }
    }

    public void FixedUpdateState()
    {

    }

    public void UpdateState()
    {
        
    }

    public void LateUpdateState()
    {
        
    }

    public void ExitState()
    {

    }
}
