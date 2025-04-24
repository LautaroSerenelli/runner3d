using UnityEngine;

public class JumpMultiplierEffect : MonoBehaviour, IPowerUpEffect
{
    [SerializeField] private float duration = 5f;

    public void Apply(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.ApplyJumpMultiplier(1.75f, duration);
        }

        Destroy(gameObject);
    }
}