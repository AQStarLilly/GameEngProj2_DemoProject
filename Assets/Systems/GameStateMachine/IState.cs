//Defines the methods required for implementing a game state in the state machine
//Each game state should implement this interface to ensure consistent behaviour

public interface IState
{
    void EnterState();

    void FixedUpdateState();

    void UpdateState();

    void LateUpdateState();

    void ExitState();
}
