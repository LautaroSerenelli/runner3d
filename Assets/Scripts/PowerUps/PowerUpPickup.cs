using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    [SerializeField] private GameObject effectPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (effectPrefab != null)
            {
                GameObject effectInstance = Instantiate(effectPrefab);
                IPowerUpEffect effect = effectInstance.GetComponent<IPowerUpEffect>();
                if (effect != null)
                {
                    effect.Apply(other.gameObject);
                }
            }

            Destroy(gameObject);
        }
    }
}