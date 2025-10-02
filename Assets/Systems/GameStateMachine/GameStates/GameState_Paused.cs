using UnityEngine;
using UnityEngine.InputSystem;

public class GameState_Paused : IState
{
    GameManager gameManager => GameManager.Instance;
    GameStateManager gameStateManager => GameManager.Instance.GameStateManager;
    UIManager UIManager => GameManager.Instance.UIManager;

    #region Singleton Instance

    private static readonly GameState_Paused instance = new GameState_Paused();

    public static GameState_Paused Instance = instance;

    #endregion


    public void EnterState()
    {
        Time.timeScale = 0f;  //pause the game
        Cursor.visible = true;
        UIManager.EnablePauseMenuUI();
    }

    public void FixedUpdateState()
    {

    }

    public void UpdateState()
    {
        if (Keyboard.current[Key.Escape].wasPressedThisFrame)
        {
            gameStateManager.Resume();
        }
    }

    public void LateUpdateState()
    {

    }

    public void ExitState()
    {

    }
}
