using UnityEngine;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Outline))]

public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    [Header("Highlight Effect Settings")]
    protected Outline outline;

    protected bool isFocused = false;

    public virtual void Awake()
    {
        if(gameObject.layer != LayerMask.NameToLayer("Interactable"))
        {
            gameObject.layer = LayerMask.NameToLayer("Interactable");
        }

        #region Initialize Highlight Effect
        outline = GetComponent<Outline>();
        outline.OutlineColor = Color.green;
        outline.OutlineWidth = 5f;
        outline.enabled = false;
        #endregion
    }


    public string GetInteractionPrompt()
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnInteract()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }

    public void SetFocus(bool focused)
    {
        if (isFocused == focused) return;

        isFocused = focused;

        if (focused)
        {
            // Perform focus logic
            outline.enabled = true;
        }
        else
        {
            // Lose focus logic
            outline.enabled = false;
        }
    }   
}
