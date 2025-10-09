using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if(player != null)
        {
            GameManager.Instance.GameStateManager.SwitchToState(GameManager.Instance.GameStateManager.gameState_GameOver);
        }
    }
}
