using UnityEngine;

public class interactableDemoBall : BaseInteractable
{
    private void Awake()
    {
        base.Awake();
    }

    public override void OnInteract()
    {
        Debug.Log("Using Logic from interactable demo ball");
        Destroy(gameObject);
    }
}
