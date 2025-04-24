using UnityEngine;

public class DeathManager : MonoBehaviour
{
    [Header("Muerte por caídas")]
    [SerializeField] private Collider deathTrigger;

    [Header("Muerte por colisión")]
    [SerializeField] private float speedLimit = -0.5f;
    [SerializeField] private float deathTime = 0.1f;
    private float actualDeathTime = 0f;
    private Vector3 lastPosition;

    [Header("Muerte por objetos peligrosos")]
    [SerializeField] private string[] dangerousObjectsTags;

    private bool isDeath = false;

    private bool isImmune = false;

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void Update()
    {
        if (isDeath) return;

        VerifyTimeDeath();
    }

    private void VerifyTimeDeath()
    {
        float deltaZ = transform.position.z - lastPosition.z;

        if (deltaZ < speedLimit)
        {
            actualDeathTime += Time.deltaTime;
        }
        else
        {
            actualDeathTime = 0f;
        }

        if (actualDeathTime >= deathTime)
        {
            MatarJugador("Muerte por choque.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathZone") && !isDeath)
        {
            MatarJugador("Muerte por caída fuera del mapa.");
        }
        else
        {
            if (isImmune) return;
            foreach (string tag in dangerousObjectsTags)
            {
                if (other.gameObject.CompareTag(tag) && !isDeath)
                {
                    MatarJugador($"Muerte por colisión con {tag}.");
                    break;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isImmune) return;

        foreach (string tag in dangerousObjectsTags)
        {
            if (collision.gameObject.CompareTag(tag) && !isDeath)
            {
                MatarJugador($"Muerte por colisión con {tag}.");
                break;
            }
        }
    }

    private void MatarJugador(string causa)
    {
        Debug.Log($"Jugador muerto: {causa}");

        isDeath = true;

        if (GameStatsManager.Instance != null)
        {
            GameStatsManager.Instance.EndGame(causa);
        }

        gameObject.SetActive(false);
    }

    public void SetImmune(bool immune, float duration = 0f)
    {
        isImmune = immune;
        
        if (duration > 0f)
        {
            StartCoroutine(ResetImmunity(duration));
        }
    }

    private System.Collections.IEnumerator ResetImmunity(float duration)
    {
        yield return new WaitForSeconds(duration);
        isImmune = false;
    }
}