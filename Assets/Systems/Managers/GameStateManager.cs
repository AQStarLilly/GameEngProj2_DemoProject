using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [Header("Debug (read only)")]
    [SerializeField] private string currentActiveState;
    [SerializeField] private string lastActiveState;

    //private variables to store state information
    private IState currentState;
    private IState lastState;

    //Instantiate GameStates
    public GameState_MainMenu gameState_MainMenu = GameState_MainMenu.Instance;
    public GameState_Gameplay gameState_Gameplay = GameState_Gameplay.Instance;


    private void Start()
    {
        currentState = gameState_Gameplay;
        currentActiveState = currentState.ToString();
        currentState.EnterState();
    }


    #region State Machine Update Calls

    private void FixedUpdate()
    {
        currentState.FixedUpdateState();
    }

    private void Update()
    {
        currentState.UpdateState();
    }

    private void LateUpdate()
    {
        currentState.LateUpdateState();
    }
    #endregion

    public void SwitchToState(IState newState)
    {
        lastState = currentState; //store the current state as the last state
        lastActiveState = lastState.ToString(); //Update debug info in inspector
        currentState?.ExitState(); //Exit the current state

        currentState = newState; //switch to the new state
        currentActiveState = currentState.ToString(); //Update debug info in inspector
        currentState.EnterState(); //Enter the new state
    }
}
