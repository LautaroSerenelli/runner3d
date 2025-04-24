using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] public float smoothSpeed = 5f;
    [SerializeField] public float yOffset = 2f;

    private Vector3 initialOffset;

    void Start()
    {
        initialOffset = transform.position - player.position;
        initialOffset.y = 0;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = new Vector3(
            player.position.x + initialOffset.x,
            player.position.y + yOffset,
            player.position.z + initialOffset.z
        );

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}