using UnityEngine;
using UnityEngine.InputSystem;

public class Jump : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpForce = 100f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("References")]
    public Rigidbody rb;
    public InputActionReference jumpAction;
    public Animator animator;

    private bool isGrounded = true;
    private bool jumpQueued = false;

    void OnEnable()
    {
        if (jumpAction != null)
            jumpAction.action.performed += OnJump;
    }

    void OnDisable()
    {
        if (jumpAction != null)
            jumpAction.action.performed -= OnJump;
    }

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (animator == null) animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (rb == null) return;

        if (jumpQueued && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            jumpQueued = false;

            if (animator != null)
                animator.SetTrigger("Jump");
        }

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !jumpQueued)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1f) * Time.deltaTime;
        }
    }

    void OnJump(InputAction.CallbackContext context)
    {
        jumpQueued = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }
}
