using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Assertions.Must;

public class PlayerController : MonoBehaviour
{
    public enum MovementState
    {
        Idle,
        Walking,
        Sprinting,
        Crouching,
        Jumping,
        Falling
    }

    public MovementState currentMovementState;

    //Manager References
    private InputManager inputManager => GameManager.Instance.InputManager;
    private CharacterController characterController => GetComponent<CharacterController>();
    
    [SerializeField] private Transform cameraRoot;
    public Transform CameraRoot => cameraRoot;


    [Header("Enable/Disable Controls & Features")]
    public bool moveEnabled = true;
    public bool lookEnabled = true;

    [SerializeField] private bool jumpEnabled = true;
    [SerializeField] private bool sprintEnabled = true;
    [SerializeField] private bool crouchEnabled = true;


    [Header("Move Settings")]
    public float moveSpeed = 5f;
    [SerializeField] private float crouchMoveSpeed = 2.0f;
    [SerializeField] private float walkMoveSpeed = 4.0f;
    [SerializeField] private float sprintMoveSpeed = 7.0f;

    [SerializeField] private float currentMoveSpeed;
    private float speedTransitionDuration = 0.25f;

    [SerializeField] private bool sprintInput = false;
    [SerializeField] private bool crouchInput = false;

    private Vector3 velocity;


    [Header("Look Settings")]
    public float horizontalLookSensitivity = 90;
    public float verticalLookSensitivity = 90;
    public float LowerLookLimit = -60;
    public float UpperLookLimit = 60;


    [Header("Jump & Gravity Settings")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private float gravity = 30.0f;
    [SerializeField] private float jumpHeight = 2.0f;

    private float jumpCooldownAmount = 0.2f;
    private float jumpCooldownTimer = 0f;
    private bool jumpRequested = false;
    //private float groundCheckRadius = 0.1f;

    [Header("Crouch Settings")]
    private float standingHeight;
    private Vector3 standingCenter;
    private float standingCamY;
    private bool isObstructed = false;

    [SerializeField] private float crouchTransitionDuration = 0.2f; // Time in seconds for crouch/stand transition (approximate completion)
    [SerializeField] private float crouchingHeight = 1.0f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private float crouchingCamY = 0.75f;

    private float targetHeight;
    private Vector3 targetCenter;
    private float targetCamY; // Target Y position for camera root during crouch transition

    private int playerLayerMask;

    public Transform spawnPosition;

    //Input Variables
    private Vector2 moveInput;
    private Vector2 lookInput;

    private void Awake()
    {
        playerLayerMask = ~LayerMask.GetMask("Player");

        #region Intialize Default Values
        currentMovementState = MovementState.Idle;
        
        // Initialize crouch variables
        standingHeight = characterController.height;
        standingCenter = characterController.center;
        standingCamY = cameraRoot.localPosition.y;

        targetHeight = standingHeight;
        targetCenter = standingCenter;
        targetCamY = cameraRoot.localPosition.y;

        crouchInput = false;
        sprintInput = false;

        #endregion
    }


    public void HandlePlayerMovement()
    {
        if (moveEnabled == false) return;

        //Determine movement state
        DetermineMovementState();

        //Perform ground check
        GroundedCheck();

        //Handle crouch transition
        HandleCrouchTransition();

        //Apply movement
        ApplyMovement();
    }

    private void DetermineMovementState()
    {
        //Determine current movement state based on inputs and conditions
        if (isGrounded == false)
        {
            if (velocity.y > 0.1f)
            {
                currentMovementState = MovementState.Jumping;
            }
            else if (velocity.y < 0)
            {
                currentMovementState = MovementState.Falling;
            }
        }
        else if (isGrounded == true)
        {
            if (crouchInput == true || isObstructed == true)
            {
                currentMovementState = MovementState.Crouching;
            }
            else if (sprintInput == true && currentMovementState != MovementState.Crouching)
            {
                currentMovementState = MovementState.Sprinting;
            }
            else if (moveInput.magnitude > 0.1f && sprintInput == false && crouchInput == false)
            {
                currentMovementState = MovementState.Walking;
            }
            else if (moveInput.magnitude <= 0.1f && sprintInput == false && crouchInput == false)
            {
                currentMovementState = MovementState.Idle;
            }
        }
    }
    
    private void ApplyMovement()
    {
        //Step 1: Get Input Direction
        Vector3 moveInputDirection = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 worldMoveDirection = transform.TransformDirection(moveInputDirection);

        //Step 2: Determine Movement Speed
        float targetMoveSpeed = currentMoveSpeed;

        switch (currentMovementState) 
        {
            case MovementState.Crouching:
                {
                    targetMoveSpeed = crouchMoveSpeed;
                    break;
                }
            case MovementState.Sprinting:
                {
                    targetMoveSpeed = sprintMoveSpeed;
                    break;
                }                
            default:
                {
                    targetMoveSpeed = walkMoveSpeed;
                    break;
                }
        }

        //Step 3: Smoothly Interpolate Current Speed towards Target Speed
        float lerpSpeed = 1f - Mathf.Pow(0.01f, Time.deltaTime / speedTransitionDuration);
        currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, targetMoveSpeed, lerpSpeed);

        //Step 4: Handle Horizontal Movement
        Vector3 horizontalMovement = worldMoveDirection * currentMoveSpeed;

        //Step 5: Handle Jumping and gravity
        ApplyJumpAndGravity();

        //Step 6: Combine Horizontal and Vertical Movement
        Vector3 movement = horizontalMovement;
        movement.y = velocity.y;

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

    private void ApplyJumpAndGravity()
    {
        if(jumpEnabled == true)
        {
            if (jumpRequested == true)
            {
                velocity.y = Mathf.Sqrt(2f * jumpHeight * gravity);

                jumpRequested = false;

                jumpCooldownTimer = jumpCooldownAmount;
            }
        }

        // Apply gravity based on the player's current state (grounded or in air).
        if (isGrounded && velocity.y < 0)
        {
            // If grounded and moving downwards (due to accumulated gravity from previous frames),
            // snap velocity to a small negative value. This keeps the character firmly on the ground
            // without allowing gravity to build up indefinitely, preventing "bouncing" or
            // incorrect ground detection issues.

            velocity.y = -1f;
        }
        else  // If not grounded (in the air):
        {
            // apply standard gravity
            velocity.y -= gravity * Time.deltaTime;
        }


        if (jumpCooldownTimer > 0)
        {
            jumpCooldownTimer -= Time.deltaTime;
        }
    }

    private void HandleCrouchTransition()
    {
        bool shouldCrouch = crouchInput == true;

        // if airborne and was crouching, maintain crouch state (prevents standing up from crouch while walking off a ledge)
        bool wasAlreadyCrouching = characterController.height < (standingHeight - 0.05f);

        if (isGrounded == false && wasAlreadyCrouching)
        {
            shouldCrouch = true; // Maintain crouch state if airborne (walking off ledge while crouching)
        }

        if (shouldCrouch)
        {
            targetHeight = crouchingHeight;
            targetCenter = crouchingCenter;
            targetCamY = crouchingCamY;
            isObstructed = false; // No obstruction when intentionally crouching
        }
        else
        {
            float maxAllowedHeight = GetMaxAllowedHeight();
           
            if (maxAllowedHeight >= standingHeight - 0.05f)
            {
                // No obstruction, allow immediate transition to standing
                targetHeight = standingHeight;
                targetCenter = standingCenter;
                targetCamY = standingCamY;
                isObstructed = false;
            }

            else
            {
                // Obstruction detected, limit height and center
                targetHeight = Mathf.Min(standingHeight, maxAllowedHeight);
                float standRatio = Mathf.Clamp01((targetHeight - crouchingHeight) / (standingHeight - crouchingHeight));
                targetCenter = Vector3.Lerp(crouchingCenter, standingCenter, standRatio);
                targetCamY = Mathf.Lerp(crouchingCamY, standingCamY, standRatio);
                isObstructed = true;
            }
        }

        // Calculate lerp speed based on desired duration
        // This formula ensures the transition approximately reaches 99% of the target in 'crouchTransitionDuration' seconds.
        float lerpSpeed = 1f - Mathf.Pow(0.01f, Time.deltaTime / crouchTransitionDuration);

        // Smoothly transition to targets
        characterController.height = Mathf.Lerp(characterController.height, targetHeight, lerpSpeed);
        characterController.center = Vector3.Lerp(characterController.center, targetCenter, lerpSpeed);

        Vector3 currentCamPos = cameraRoot.localPosition;
        cameraRoot.localPosition = new Vector3(currentCamPos.x, Mathf.Lerp(currentCamPos.y, targetCamY, lerpSpeed), currentCamPos.z);

    }


    #region Helper Methods

    private void GroundedCheck()
    {
        isGrounded = characterController.isGrounded;
    }

    private float GetMaxAllowedHeight()
    {
        //Cast a ray upwards from the character's position to check for obstructions
        RaycastHit hit;
        float maxCheckDistance = standingHeight + 0.15f;

        if (Physics.Raycast(transform.position, Vector3.up, out hit, maxCheckDistance, playerLayerMask))
        {
            //we hit something so calculate the max height the player can stand
            float maxHeight = hit.distance - 0.1f;

            maxHeight = Mathf.Max(maxHeight, crouchingHeight);

            return maxHeight; 
        }
        return standingHeight;
    }

    public void MovePlayerToSpawnPosition(Transform spawnPosition)
    {
        characterController.enabled = false;
        transform.position = spawnPosition.position;
        transform.rotation = spawnPosition.rotation;
        characterController.enabled = true;
    }

    #endregion

    #region Input Methods

    void SetMoveInput(Vector2 inputVector)
    {
        moveInput = new Vector2(inputVector.x, inputVector.y);
    }

    void SetLookEvent(Vector2 inputVector)
    {
        lookInput = new Vector2(inputVector.x, inputVector.y);
        //Debug.Log($"Look Input: {lookInput}");
    }

    void JumpInputEvent(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Jump input started");

            if (jumpEnabled && isGrounded && jumpCooldownTimer <= 0f)
            {
                jumpRequested = true;

                jumpCooldownTimer = 0.1f;
            }
        }
        
    } 
    
    void CrouchInputEvent(InputAction.CallbackContext context)
    {
        if (crouchEnabled == false) return;

        if (context.started)
        {
            crouchInput = !crouchInput;

            Debug.Log("Crouch Input started");
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
