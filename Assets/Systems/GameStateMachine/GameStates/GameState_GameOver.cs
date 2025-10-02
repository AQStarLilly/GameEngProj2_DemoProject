using UnityEngine;
using UnityEngine.InputSystem;

public class GameState_GameOver : IState
{
    GameManager gameManager => GameManager.Instance;
    GameStateManager gameStateManager => GameManager.Instance.GameStateManager;
    UIManager UIManager => GameManager.Instance.UIManager;

    #region Singleton Instance

    private static readonly GameState_GameOver instance = new GameState_GameOver();

    public static GameState_GameOver Instance = instance;

    #endregion


    public void EnterState()
    {
        Time.timeScale = 0f;  //pause the game
        Cursor.visible = true;
        UIManager.EnableGameOverUI();
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