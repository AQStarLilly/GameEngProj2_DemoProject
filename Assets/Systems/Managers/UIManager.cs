using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem.XR.Haptics;

public class UIManager : MonoBehaviour
{
    [Header("UI Menu Objects")]
    [SerializeField] private UIDocument mainMenuUI;
    [SerializeField] private UIDocument gameplayUI;
    [SerializeField] private UIDocument pauseMenuUI;

    [Header("Interaction Popup Settings")]
    [SerializeField] private float popupDisplayTime = 2f; // how long popup stays visible
    private Label interactionPopupLabel;
    private Coroutine popupCoroutine;

    public void Awake()
    {
        mainMenuUI = FindUIDocument("MainMenuUI");
        gameplayUI = FindUIDocument("GameplayUI");
        pauseMenuUI = FindUIDocument("PauseMenuUI");

        if (mainMenuUI != null) mainMenuUI.gameObject.SetActive(true);
        if (gameplayUI != null) gameplayUI.gameObject.SetActive(true);
        if (pauseMenuUI != null) pauseMenuUI.gameObject.SetActive(true);

        DisableAllMenuUI();

        // Try to find popup label when gameplay UI exists
        if (gameplayUI != null)
        {
            var root = gameplayUI.rootVisualElement;
            interactionPopupLabel = root.Q<Label>("interactionPopup");
            if (interactionPopupLabel != null)
            {
                interactionPopupLabel.style.opacity = 0; // start hidden
            }
            else
            {
                Debug.LogWarning("'interactionPopup' Label not found in GameplayUI. Make sure your UXML has a Label named 'interactionPopup'.");
            }
        }
    }

    public void DisableAllMenuUI()
    {
        mainMenuUI.rootVisualElement.style.display = DisplayStyle.None;
        gameplayUI.rootVisualElement.style.display = DisplayStyle.None;
        pauseMenuUI.rootVisualElement.style.display = DisplayStyle.None;
        //gameOverUI.SetActive(false);
    }

    public void EnableMainMenuUI()
    {
        DisableAllMenuUI();
        mainMenuUI.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    public void EnableGameplayUI()
    {
        DisableAllMenuUI();
        gameplayUI.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    public void EnablePauseMenuUI()
    {
        DisableAllMenuUI();
        pauseMenuUI.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    public void EnableGameOverUI()
    {
        DisableAllMenuUI();
        //gameOverUI.SetActive(true);
    }

    private UIDocument FindUIDocument(string name)
    {
        var documents = Object.FindObjectsByType<UIDocument>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var doc in documents)
        {
            if (doc.name == name)
            {
                return doc;
            }
        }
        Debug.LogWarning($"UIDocument '{name}' not found in scene.");
        return null;
    }

    // ================================
    // INTERACTION POPUP METHODS BELOW
    // ================================

    /// <summary>
    /// Shows a temporary text popup on the gameplay UI.
    /// </summary>
    public void ShowInteractionPopup(string message)
    {
        if (interactionPopupLabel == null)
        {
            Debug.LogWarning("Cannot show popup — interactionPopupLabel not found.");
            return;
        }

        // Stop any currently running popup hide coroutine
        if (popupCoroutine != null)
            StopCoroutine(popupCoroutine);

        interactionPopupLabel.text = message;
        interactionPopupLabel.style.opacity = 1f;

        popupCoroutine = StartCoroutine(HidePopupAfterDelay());
    }

    private IEnumerator HidePopupAfterDelay()
    {
        yield return new WaitForSeconds(popupDisplayTime);
        interactionPopupLabel.style.opacity = 0f;
    }
}
