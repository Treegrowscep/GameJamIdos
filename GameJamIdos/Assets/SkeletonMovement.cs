using UnityEngine;
using UnityEngine.InputSystem;

public class SkeletonMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform cameraTransform;

    private Vector2 moveInput;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
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

        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // Управление анимацией
        animator.SetFloat("Speed", moveDirection.magnitude);
    }
}