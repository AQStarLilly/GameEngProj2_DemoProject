using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuUIController : MonoBehaviour
{
    private UIDocument pauseMenuUIDoc => GetComponent<UIDocument>();

    GameManager gameManage => GameManager.Instance;
    UIManager UIManager => GameManager.Instance.UIManager;
    LevelManager levelManager => GameManager.Instance.LevelManager;
    InputManager inputManager => GameManager.Instance.InputManager;
    GameStateManager gameStateManager => GameManager.Instance.GameStateManager;

    Button resumeButton;
    Button menuButton;
    

    private Button[] pauseButtons;
    private int focusedIndex = 0;


    private void OnEnable()
    {
        resumeButton = pauseMenuUIDoc.rootVisualElement.Q<Button>("ResumeButton");
        menuButton = pauseMenuUIDoc.rootVisualElement.Q<Button>("MenuButton");       


        resumeButton.clicked += OnResumeButtonClicked;
        menuButton.clicked += OnMenuButtonClicked;
    }

    private void OnDestroy()
    {
        resumeButton.clicked -= OnResumeButtonClicked;
        menuButton.clicked -= OnMenuButtonClicked;
    }

    private void OnResumeButtonClicked()
    {
        gameStateManager.Resume();
    }

    private void OnMenuButtonClicked()
    {
        levelManager.LoadMainMenu();
    }
}
