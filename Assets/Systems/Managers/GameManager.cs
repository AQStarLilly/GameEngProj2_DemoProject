using UnityEngine;


//GameManager must load first to initialize it's references
[DefaultExecutionOrder(-100)]

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Manager References")]
    public InputManager inputManager;


    private void Awake()
    {
        #region Singleton
        //Singleton to ensure only one gameManager exists

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        #endregion

        if(inputManager == null)
        {
            inputManager = GetComponentInChildren<InputManager>();
        }
    }

}