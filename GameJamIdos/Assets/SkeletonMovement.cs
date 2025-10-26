using UnityEngine;
using UnityEngine.InputSystem;

public class SkeletonMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public Transform cameraTransform;

    private Vector2 moveInput;
    private bool isRunning = false;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnRun(InputValue value)
    {
        isRunning = value.isPressed;
    }

    private void Update()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * moveInput.y + right * moveInput.x;
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        transform.position += moveDirection * currentSpeed * Time.deltaTime;

        // Анимация: скорость движения
        animator.SetFloat("Speed", moveDirection.magnitude * (isRunning ? 2f : 1f));
    }
}