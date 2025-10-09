using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    [Header("Target Scene")]
    public string targetSceneName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.LevelManager.LoadLevel(targetSceneName);
        }
    }
}
