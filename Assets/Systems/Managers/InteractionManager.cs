using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private InputManager inputManager => GameManager.Instance.InputManager;

    [Header("Interaction Settings")]
    private LayerMask interactableLayer;
    [SerializeField] private float interactionDistance = 3f;

    // Interface reference used internally
    [SerializeField] private IInteractable currentFocusedInteractable;

    private Transform cameraRoot; //Reference to player camera root transform


    private void Start()
    {
        //Set the interactable layer
        interactableLayer = LayerMask.GetMask("Interactable");

        //Set the camera root from the player controller
        cameraRoot = GameManager.Instance.PlayerController.CameraRoot;
        
    }
    private void Update()
    {
        HandleInteractionDetection();
    }

    private void HandleInteractionDetection()
    {
        if(Physics.Raycast(cameraRoot.transform.position, cameraRoot.transform.forward, out RaycastHit hitInfo, interactionDistance, interactableLayer))
        {
            Debug.Log(hitInfo.collider.name);

            //Get the interactable commponent to the hit object
            IInteractable hitInteractable = hitInfo.collider.GetComponent<IInteractable>();

            if(hitInteractable != null)
            {
                if(hitInteractable != currentFocusedInteractable)
                {
                    if(currentFocusedInteractable != null)
                    {
                        currentFocusedInteractable.SetFocus(false);
                    }

                    //2. set new focus 
                    currentFocusedInteractable = hitInteractable;
                    currentFocusedInteractable.SetFocus(true);

                    //3. Get the prompt text from interactable and tell the UI to show it

                    //Use reference to UI text to pass
                }              
            }           
        }
        else if (currentFocusedInteractable != null)
        {
            currentFocusedInteractable.SetFocus(false);
            currentFocusedInteractable = null;
        }

    }


    private void OnInteractInput(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(currentFocusedInteractable != null)
            {
                currentFocusedInteractable.OnInteract();
            }
        }
    }

    private void OnEnable()
    {
        inputManager.InteractInputEvent += OnInteractInput;
    }

    private void OnDestroy()
    {
        inputManager.InteractInputEvent -= OnInteractInput;
    }
}
