using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{

    public GameObject mainMenuUI;
    public GameObject gameplayUI;
    public GameObject pauseMenuUI;
    public GameObject gameOverUI;

    public void Awake()
    {
        DisableAllMenuUI();
    }

    public void DisableAllMenuUI()
    {
        mainMenuUI.SetActive(false);
        gameplayUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
    }

    public void EnableMainMenuUI()
    {
        DisableAllMenuUI();
        mainMenuUI.SetActive(true);
    }

    public void EnableGameplayUI()
    {
        DisableAllMenuUI();
        gameplayUI.SetActive(true);
    }

    public void EnablePauseMenuUI()
    {
        DisableAllMenuUI();
        pauseMenuUI.SetActive(true);
    }

    public void EnableGameOverUI()
    {
        DisableAllMenuUI();
        gameOverUI.SetActive(true);
    }
}
