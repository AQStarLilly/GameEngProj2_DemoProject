using UnityEngine;
using UnityEngine.InputSystem;

public class GameState_Gameplay : IState
{
    GameManager gameManager => GameManager.Instance;
    GameStateManager gameStateManager => GameManager.Instance.GameStateManager;

    PlayerController playerController => GameManager.Instance.PlayerController;
    UIManager UIManager => GameManager.Instance.UIManager;
    #region Singleton Instance

    private static readonly GameState_Gameplay instance = new GameState_Gameplay();

    public static GameState_Gameplay Instance = instance;

    #endregion


    public void EnterState()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        UIManager.EnableGameplayUI();       
    }

    public void FixedUpdateState()
    {

    }

    public void UpdateState()
    {
        playerController.HandlePlayerMovement();
      
        if (Keyboard.current[Key.Escape].wasPressedThisFrame)
        {
            gameStateManager.Pause();
        }
    }

    public void LateUpdateState()
    {
        playerController.HandlePlayerLook();
    }

    public void ExitState()
    {

    }
}
