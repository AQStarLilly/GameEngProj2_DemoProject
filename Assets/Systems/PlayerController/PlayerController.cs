using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Manager References
    private InputManager inputManager => GameManager.Instance.inputManager;
    private CharacterController characterController => GetComponent<CharacterController>();
    
    [SerializeField] private Transform cameraRoot;
    public Transform CameraRoot => cameraRoot;


    [Header("Enable/Disable Controls & Features")]
    public bool moveEnabled = true;
    public bool lookEnabled = true;

    [SerializeField] private bool jumpEnabled = true;
    [SerializeField] private bool sprintEnabled = true;


    //Input Variables
    private Vector2 moveInput;
    private Vector2 lookInput;

    public float moveSpeed = 5f;
    [SerializeField] private float crouchMoveSpeed = 2.0f;
    [SerializeField] private float walkMoveSpeed = 4.0f;
    [SerializeField] private float sprintMoveSpeed = 7.0f;

    [SerializeField] private float currentMoveSpeed;
    private float speedTransitionDuration = 0.25f;

    [SerializeField] private bool sprintInput = false;
    private bool crouchInput = false;

    [Header("Look Settings")]
    public float horizontalLookSensitivity = 90;
    public float verticalLookSensitivity = 90;

    public float LowerLookLimit = -60;
    public float UpperLookLimit = 60;



    private void Awake()
    {
        
    }

    private void Update()
    {
        HandlePlayerMovement();
    }

    private void LateUpdate()
    {
        HandlePlayerLook();
    }

    public void HandlePlayerMovement()
    {
        if (moveEnabled == false) return;

        //Step 1: Get Input Direction
        Vector3 moveInputDirection = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 worldMoveDirection = transform.TransformDirection(moveInputDirection);

        //Step 2: Determine Movement Speed
        float targetMoveSpeed = moveSpeed;

        if(sprintInput == true)
        {
            targetMoveSpeed = sprintMoveSpeed;
        }
        else
        {
            targetMoveSpeed = walkMoveSpeed;
        }

        //Step 3: Smoothly Interpolate Current Speed towards Target Speed
        float lerpSpeed = 1f - Mathf.Pow(0.01f, Time.deltaTime / speedTransitionDuration);
        currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, targetMoveSpeed, lerpSpeed);

        //Step 4: Handle Horizontal Movement
        Vector3 horizontalMovement = worldMoveDirection * currentMoveSpeed;

        //Step 5: Handle Jumping and gravity


        //Step 6: Combine Horizontal and Vertical Movement
        Vector3 movement = horizontalMovement;

        //Step 7: Apply Final Movement
        characterController.Move(movement * Time.deltaTime);
    }

    public void HandlePlayerLook()
    {
        if (lookEnabled == false) return;

        float lookX = lookInput.x * horizontalLookSensitivity * Time.deltaTime;
        float lookY = lookInput.y * verticalLookSensitivity * Time.deltaTime;

        //Rotate Character on X-Axis
        transform.Rotate(Vector3.up, lookX);

        //Tilt cameraRoot on X-Axis
        Vector3 currentAngles = cameraRoot.localEulerAngles;
        float newRotationX = currentAngles.x - lookY;

        //Convert to Signed Angle for Proper Clamping
        newRotationX = (newRotationX > 180) ? newRotationX - 360 : newRotationX;
        newRotationX = Mathf.Clamp(newRotationX, LowerLookLimit, UpperLookLimit);


        CameraRoot.localEulerAngles = new Vector3(newRotationX, 0, 0);
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
        if (sprintEnabled == false) return;

        if (context.started)
        {
            sprintInput = true;
        }
        else if (context.canceled)
        {
            sprintInput = false;
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
