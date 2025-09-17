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

    public event Action<InputAction.CallbackContext> JumpInputEvent;
    public event Action<InputAction.CallbackContext> CrouchInputEvent;
    public event Action<InputAction.CallbackContext> SprintInputEvent;


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
        JumpInputEvent?.Invoke(context);
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        CrouchInputEvent?.Invoke(context);
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        SprintInputEvent?.Invoke(context);
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
