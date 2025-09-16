using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, Inputs.IPlayerActions
{
    private Inputs inputs;

    void Awake()
    {
        inputs = new Inputs();
        inputs.Player.SetCallbacks(this); //Set callbacks for player action map
        inputs.Player.Enable(); //Enables the "player" action map       
    }

    #region Input Events
    //Events that are triggered when inputs are detected
    public event Action<Vector2> MoveInputEvent;
    public event Action<Vector2> LookInputEvent;

    public event Action JumpStartedInputEvent;
    public event Action JumpPerformedInputEvent;
    public event Action JumpCanceledInputEvent;

    public event Action CrouchStartedInputEvent;
    public event Action CrouchPerformedInputEvent;
    public event Action CrouchCanceledInputEvent;

    public event Action SprintStartedInputEvent;
    public event Action SprintPerformedInputEvent;
    public event Action SprintCanceledInputEvent;

    #endregion

    #region Input Callbacks
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInputEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookInputEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpStartedInputEvent?.Invoke();
        }
        if (context.performed)
        {
            JumpPerformedInputEvent?.Invoke();
        }
        if (context.canceled)
        {
            JumpCanceledInputEvent?.Invoke();
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            CrouchStartedInputEvent?.Invoke();
        }
        if (context.performed)
        {
            CrouchPerformedInputEvent?.Invoke();
        }
        if (context.canceled)
        {
            CrouchCanceledInputEvent?.Invoke();
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SprintStartedInputEvent?.Invoke();
        }
        if (context.performed)
        {
            SprintPerformedInputEvent?.Invoke();
        }
        if (context.canceled)
        {
            SprintCanceledInputEvent?.Invoke();
        }
    }

    #endregion



    void OnEnable()
    {
        if(inputs != null)
        {
            inputs.Player.Enable();
        }
    }

    void OnDestroy()
    {
        if (inputs != null)
        {
            inputs.Player.Disable();
        }
    }
}
