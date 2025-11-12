using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUIController : MonoBehaviour
{
    private UIDocument mainMenuUIDoc => GetComponent<UIDocument>();

    GameManager gameManage => GameManager.Instance;
    UIManager UIManager => GameManager.Instance.UIManager;
    LevelManager levelManager => GameManager.Instance.LevelManager;
    InputManager inputManager => GameManager.Instance.InputManager;
    GameStateManager gameStateManager => GameManager.Instance.GameStateManager;

    Button playButton;
    Button optionsButton;
    Button quitButton;

    private Button[] menuButtons;
    private int focusedIndex = 0;
    

    private void OnEnable()
    {
        playButton = mainMenuUIDoc.rootVisualElement.Q<Button>("PlayButton");
        optionsButton = mainMenuUIDoc.rootVisualElement.Q<Button>("OptionsButton");
        quitButton = mainMenuUIDoc.rootVisualElement.Q<Button>("QuitButton");


        playButton.clicked += OnPlayButtonClicked;
        optionsButton.clicked += OptionsButton_clicked;
        quitButton.clicked += QuitButtonClicked;
    }

    private void OnDestroy()
    {
        playButton.clicked -= OnPlayButtonClicked;
        optionsButton.clicked -= OptionsButton_clicked;
        quitButton.clicked -= QuitButtonClicked;
    }

    private void OptionsButton_clicked()
    {
        throw new System.NotImplementedException();
    }

    private void OnPlayButtonClicked()
    {
        levelManager.LoadScene(2);
    }

    private void QuitButtonClicked()
    {
        Application.Quit();
    }
    
}
