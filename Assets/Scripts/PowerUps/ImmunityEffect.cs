using UnityEngine;

public class ImmunityEffect : MonoBehaviour, IPowerUpEffect
{
    [SerializeField] private float duration = 8f;

    public void Apply(GameObject player)
    {
        DeathManager deathManager = player.GetComponent<DeathManager>();
        if (deathManager != null)
        {
            deathManager.SetImmune(true, duration);
        }
        Destroy(gameObject);
    }
}