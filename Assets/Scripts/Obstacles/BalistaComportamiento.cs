using UnityEngine;

public class BalistaComportamiento : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform flecha;
    [SerializeField] private float rangoDisparo = 10f;
    [SerializeField] private float fuerzaDisparo = 20f;

    private bool puedeDisparar = true;

    private Collider balistaCollider;
    private Collider flechaCollider;

    private Rigidbody flechaRb;

    private void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        flechaRb = flecha.GetComponent<Rigidbody>();

        if (flechaRb != null)
        {
            flechaRb.isKinematic = true;
            flechaRb.useGravity = false;
        }
    }

    private void Update()
    {
        if (player == null || flecha == null) return;

        float distancia = Vector3.Distance(transform.position, player.position);
        
        if (distancia <= rangoDisparo && puedeDisparar)
        {
            DispararFlecha();

            puedeDisparar = false;
        }

        balistaCollider = GetComponent<Collider>();
        flechaCollider = flecha.GetComponent<Collider>();

        if (balistaCollider != null && flechaCollider != null)
        {
            Physics.IgnoreCollision(balistaCollider, flechaCollider, true);
        }
    }

    private void DispararFlecha()
    {
        if (flechaRb != null)
        {
            flechaRb.isKinematic = false;
            flechaRb.useGravity = true;
            flechaRb.linearVelocity = -transform.right * fuerzaDisparo;
        }
    }
}