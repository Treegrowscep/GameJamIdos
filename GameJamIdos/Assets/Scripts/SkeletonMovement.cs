using UnityEngine;
using UnityEngine.InputSystem;

public class SkeletonMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float jumpForce = 6f;
    public Transform cameraTransform;

    private Vector2 moveInput;
    private bool isRunning = false;
    private bool isGrounded = true;

    private Animator animator;
    private Rigidbody rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded && rb != null)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            if (animator != null) animator.SetBool("IsJumping", true);
            isGrounded = false;
        }
    }

    private void Update()
    {
        // real-time check of Shift (InputSystem)
        isRunning = Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed;

        // update animator speed parameter
        float inputMagnitude = new Vector2(moveInput.x, moveInput.y).magnitude;
        if (animator != null)
            animator.SetFloat("Speed", inputMagnitude * (isRunning ? 2f : 1f));
    }

    private void FixedUpdate()
    {
        if (rb == null) return;
        if (cameraTransform == null)
        {
            // fallback to world-relative movement
            Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);
            float currentSpeed = isRunning ? runSpeed : walkSpeed;
            rb.MovePosition(rb.position + move * currentSpeed * Time.fixedDeltaTime);
            return;
        }

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * moveInput.y + right * moveInput.x;
        float currentSpeed2 = isRunning ? runSpeed : walkSpeed;

        rb.MovePosition(rb.position + moveDirection * currentSpeed2 * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            if (animator != null) animator.SetBool("IsJumping", false);
        }
    }
}