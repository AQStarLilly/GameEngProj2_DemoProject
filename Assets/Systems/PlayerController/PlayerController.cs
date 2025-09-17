using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Manager References
    private InputManager inputManager;

    //Input Variables
    public Vector2 moveInput;
    public Vector2 lookInput;

    private void Awake()
    {
        inputManager = GameManager.Instance.inputManager;
    }



    #region Input Methods

    void SetMoveInput(Vector2 inputVector)
    {
        moveInput = new Vector2(inputVector.x, inputVector.y);
    }

    void SetLookEvent(Vector2 inputVector)
    {
        lookInput = new Vector2(inputVector.x, inputVector.y);
        Debug.Log($"Look Input: {lookInput}");
    }

    void JumpInputEvent(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Jump Started");
        }
        if (context.performed)
        {
            Debug.Log("Hold");
        }
    } 
    
    void CrouchInputEvent(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Crouch Started");
        }
    }

    void SprintInputEvent(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Sprint Started");
        }
    }

    #endregion

    void OnEnable()
    {
        inputManager.MoveInputEvent += SetMoveInput;
        inputManager.LookInputEvent += SetLookEvent;

        inputManager.JumpInputEvent += JumpInputEvent;
        inputManager.CrouchInputEvent += CrouchInputEvent;
        inputManager.SprintInputEvent += SprintInputEvent;
    }

    void OnDestroy()
    {
        inputManager.MoveInputEvent -= SetMoveInput;
        inputManager.LookInputEvent -= SetLookEvent;

        inputManager.JumpInputEvent -= JumpInputEvent;
        inputManager.CrouchInputEvent -= CrouchInputEvent;
        inputManager.SprintInputEvent -= SprintInputEvent;
    }

}
