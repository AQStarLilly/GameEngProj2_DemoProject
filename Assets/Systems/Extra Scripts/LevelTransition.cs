using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    [Header("Target Scene")]
    public int targetId;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.LevelManager.LoadScene(targetId);
        }
    }
}
