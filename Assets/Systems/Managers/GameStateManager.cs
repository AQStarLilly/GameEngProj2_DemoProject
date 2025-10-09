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
    public GameState_Paused gameState_Paused = GameState_Paused.Instance;
    public GameState_GameOver gameState_GameOver = GameState_GameOver.Instance;


    private void Start()
    {
        currentState = gameState_MainMenu;
        currentActiveState = currentState.ToString();
        currentState.EnterState();
    }

    public void SwitchToState(IState newState)
    {
        lastState = currentState; //store the current state as the last state
        
        if (lastState != null)
        {
            lastActiveState = lastState.ToString();
            lastState.ExitState();
        }
        else
        {
            lastActiveState = "None";
        }

        currentState = newState; //switch to the new state
        currentActiveState = currentState.ToString(); //Update debug info in inspector
        currentState.EnterState(); //Enter the new state
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


    #region Button Call Methods
    
    public void Pause()
    {
        if (currentState != gameState_Gameplay)
            return;

        if (currentState == gameState_Gameplay)
        {
            SwitchToState(gameState_Paused);
            return;
        }
    }

    public void Resume()
    {
        if (currentState != gameState_Paused)
            return;

        if (currentState == gameState_Paused)
        {
            SwitchToState(gameState_Gameplay);
            return;
        }
    }

    public void Play()
    {
        SwitchToState(gameState_Gameplay);
    }

    public void Restart()
    { 
        SwitchToState(gameState_Gameplay);
    }

    public void MainMenu()
    {
        SwitchToState(gameState_MainMenu);
    }

    public void Quit()
    {
        Application.Quit();
    }



    #endregion
}
