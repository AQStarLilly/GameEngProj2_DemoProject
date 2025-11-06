using UnityEngine;
using UnityEngine.UIElements;

public class LoadingUIController : MonoBehaviour
{
    private UIDocument LoadingUIDoc => GetComponent<UIDocument>();

    GameManager gameManage => GameManager.Instance;
    UIManager UIManager => GameManager.Instance.UIManager;
    LevelManager levelManager => GameManager.Instance.LevelManager;
    InputManager inputManager => GameManager.Instance.InputManager;
    GameStateManager gameStateManager => GameManager.Instance.GameStateManager;

    private ProgressBar progressBar;

    private void OnEnable()
    {
        progressBar = LoadingUIDoc.rootVisualElement.Q<ProgressBar>("Loading Progress Bar");

        if(progressBar == null) Debug.LogError("Loading Progress not found");
    }

    public void UpdateProgressBar(float progress)
    {
        progressBar.value = progress;
    }
}
