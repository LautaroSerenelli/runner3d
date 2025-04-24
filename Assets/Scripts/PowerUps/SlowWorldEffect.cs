using UnityEngine;

public class SlowWorldEffect : MonoBehaviour, IPowerUpEffect
{
    [SerializeField] private float duration = 5f;

    public void Apply(GameObject player)
    {
        WorldGenerator worldGen = FindFirstObjectByType<WorldGenerator>();

        if (worldGen != null)
        {
            worldGen.ApplySpeedModifier(0.5f, duration);
        }

        Destroy(gameObject);
    }
}