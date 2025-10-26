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

    // Удалён OnRun — теперь Shift проверяется напрямую в Update()

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetBool("IsJumping", true);
            isGrounded = false;
        }
    }

    private void Update()
    {
        // Проверка Shift в реальном времени
        isRunning = Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * moveInput.y + right * moveInput.x;
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        rb.MovePosition(rb.position + moveDirection * currentSpeed * Time.deltaTime);

        animator.SetFloat("Speed", moveDirection.magnitude * (isRunning ? 2f : 1f));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("IsJumping", false);
        }
    }
}