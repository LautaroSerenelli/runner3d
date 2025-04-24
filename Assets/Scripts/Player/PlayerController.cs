using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float sideSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float accelerationTime = 0.2f;
    [SerializeField] private float decelerationTime = 0.1f;

    [SerializeField] private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter = 0f;

    private Vector3 currentVelocity;

    public LayerMask groundLayer;
    private Rigidbody rb;
    private Vector2 moveInput;

    private float groundCheckDistance = 1.3f;

    private float jumpMultiplier = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        // Salto
        if (jumpBufferCounter > 0f && IsGrounded())
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce * jumpMultiplier, ForceMode.Impulse);
            jumpBufferCounter = 0f;
        }

        // Mov. Horizontal
        Vector3 targetVelocity = new Vector3(moveInput.x * sideSpeed, rb.linearVelocity.y, 0);
        
        if (moveInput.x != 0)
        {
            // Aceleración
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, targetVelocity.x, sideSpeed / accelerationTime * Time.deltaTime);
        }
        else
        {
            // Desaceleración
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0, sideSpeed / decelerationTime * Time.deltaTime);
        }

        rb.linearVelocity = new Vector3(currentVelocity.x, rb.linearVelocity.y, 0);


        if (jumpBufferCounter > 0f)
        {
            jumpBufferCounter -= Time.fixedDeltaTime;
        }
    }

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    public void RequestJump()
    {
        jumpBufferCounter = jumpBufferTime;
    }

    public void ForceStopMovement()
    {
        moveInput = Vector2.zero;
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }

    public void ApplyJumpMultiplier(float multiplier, float duration)
    {
        jumpMultiplier = multiplier;
        StartCoroutine(ResetJumpMultiplier(duration));
    }

    private System.Collections.IEnumerator ResetJumpMultiplier(float duration)
    {
        yield return new WaitForSeconds(duration);
        jumpMultiplier = 1f;
    }
}