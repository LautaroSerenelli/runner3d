using System.Collections;
using UnityEngine;

public class TreeComportamiento : Obstacle
{
    [SerializeField] private float rollingSpeed = 5f;
    [SerializeField] private float torqueAmount = 10f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        StartCoroutine(ApplyRotationAndFreeze());
    }

    private IEnumerator ApplyRotationAndFreeze()
    {
        yield return null;

        rb.constraints |= RigidbodyConstraints.FreezeRotationY;

        rb.AddForce(Vector3.back * rollingSpeed, ForceMode.Impulse);
        rb.AddTorque(Vector3.forward * -torqueAmount, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.back * rollingSpeed * Time.fixedDeltaTime, ForceMode.Force);
        rb.AddTorque(Vector3.forward * -torqueAmount * Time.fixedDeltaTime, ForceMode.Force);
    }
}