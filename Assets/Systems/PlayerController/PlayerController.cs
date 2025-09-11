using UnityEngine;
using System.Collections.Generic;

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

    void JumpStartedInput()
    {
        Debug.Log("Jump Started");
    } 
    void JumpPerformedInput()
    {
        Debug.Log("Jump Performed");
    }
    void JumpCanceledInput()
    {
        Debug.Log("Jump Canceled");
    }


    #endregion

    void OnEnable()
    {
        inputManager.MoveInputEvent += SetMoveInput;
        inputManager.LookInputEvent += SetLookEvent;

        inputManager.JumpStartedInputEvent += JumpStartedInput;
        inputManager.JumpPerformedInputEvent += JumpPerformedInput;
        inputManager.JumpCanceledInputEvent += JumpCanceledInput;
    }

    void OnDestroy()
    {
        inputManager.MoveInputEvent -= SetMoveInput;
        inputManager.LookInputEvent -= SetLookEvent;

        inputManager.JumpStartedInputEvent -= JumpStartedInput;
        inputManager.JumpPerformedInputEvent -= JumpPerformedInput;
        inputManager.JumpCanceledInputEvent -= JumpCanceledInput;
    }

}
