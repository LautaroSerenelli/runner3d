using UnityEngine;

public class ScoreMultiplierEffect : MonoBehaviour, IPowerUpEffect
{
    [SerializeField] private float duration = 5f;

    public void Apply(GameObject player)
    {
        ScoreMultiplierManager.Instance.ApplyMultiplier(2f, duration);
        Destroy(gameObject);
    }
}