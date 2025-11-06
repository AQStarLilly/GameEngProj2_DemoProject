using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public static class PerformBootload
{
    const string sceneName = "BootLoader";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {      
        if(SceneManager.GetActiveScene().name != sceneName)
        {
            //check all currently loaded scenes to see if the bootstrap scene is already loaded
            for (int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
            {
                var candidateScene = SceneManager.GetSceneAt(sceneIndex);

                if (candidateScene.name == sceneName)
                {
                    return;
                }
            }

            Debug.Log("Loading Bootloader Scene" + sceneName);

            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }

}


public class BootLoader : MonoBehaviour
{
    public static BootLoader Instance { get; private set; } = null;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void Test()
    {
        Debug.Log("Bootloader scene is Active!");
    }
}