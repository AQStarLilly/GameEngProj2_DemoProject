using UnityEngine;
using UnityEngine.InputSystem;

public class GameState_MainMenu : IState
{
    GameManager gameManager => GameManager.Instance;
    GameStateManager gameStateManager => GameManager.Instance.GameStateManager;

    #region Singleton Instance

    private static readonly GameState_MainMenu instance = new GameState_MainMenu();

    public static GameState_MainMenu Instance = instance;

    #endregion


    public void EnterState()
    {
        Debug.Log("Enter Main Menu State");
    }

    public void FixedUpdateState()
    {

    }

    public void UpdateState()
    {
        Debug.Log("Running Main Menu Update State");
        if (Keyboard.current[Key.P].wasPressedThisFrame)
        {
            gameStateManager.SwitchToState(GameState_Gameplay.Instance);
        }
    }

    public void LateUpdateState()
    {

    }

    public void ExitState()
    {
        Debug.Log("Exiting Main Menu State");
    }
}
