using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [Header("UI Menu Objects")]
    [SerializeField] private UIDocument mainMenuUI;
    [SerializeField] private UIDocument gameplayUI;
    [SerializeField] private UIDocument pauseMenuUI;

    public void Awake()
    {
        mainMenuUI = FindUIDocument("MainMenuUI");
        gameplayUI = FindUIDocument("GameplayUI");
        pauseMenuUI = FindUIDocument("PauseMenuUI");

        if (mainMenuUI != null) mainMenuUI.gameObject.SetActive(true);
        if (gameplayUI != null) gameplayUI.gameObject.SetActive(true);
        if (pauseMenuUI != null) pauseMenuUI.gameObject.SetActive(true);


        DisableAllMenuUI();
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
}
