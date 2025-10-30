using UnityEngine;

public interface IInteractable 
{
    void OnInteract();

    void SetFocus(bool focused);

    string GetInteractionPrompt();
    
}
