using UnityEngine;
using UnityEngine.InputSystem;

public class GameState_Loading : IState
{
    GameManager gameManager => GameManager.Instance;
    GameStateManager gameStateManager => GameManager.Instance.GameStateManager;

    PlayerController playerController => GameManager.Instance.PlayerController;
    UIManager UIManager => GameManager.Instance.UIManager;
    #region Singleton Instance

    private static readonly GameState_Loading instance = new GameState_Loading();

    public static GameState_Loading Instance = instance;

    #endregion


    public void EnterState()
    {
        Debug.Log("Entered GameState_Loading");
        UIManager.EnableLoadingUI();
        Time.timeScale = 1f;
        Cursor.visible = false;       
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
        Debug.Log("Exited GameState_Loading");
    }
}
