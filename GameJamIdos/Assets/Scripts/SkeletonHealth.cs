using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SkeletonHealth : MonoBehaviour
{
    public Image heartIcon;
    public Camera mainCamera;
    public Camera deathCamera;

    [Tooltip("Set true for the player so camera switching and CameraLook disabling happens only for player objects")]
    public bool isPlayer = false;

    [Tooltip("Optional explicit reference to the CameraLook script to disable on death")]
    public CameraLook cameraLook;

    private bool isAlive = true;
    private bool deathHandled = false;

    // cached components
    private Animator animator;
    private SkeletonMovement movement;
    private CharacterController controller;
    private Rigidbody rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<SkeletonMovement>();
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();

        // Ensure main/death camera states: main on, death off
        if (mainCamera != null) mainCamera.enabled = true;
        if (deathCamera != null) deathCamera.enabled = false;
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.kKey.wasPressedThisFrame)
        {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        if (!isAlive || deathHandled) return;
        isAlive = false;
        deathHandled = true;

        // Disable heart icon
        if (heartIcon != null) heartIcon.enabled = false;

        // Play death animation
        if (animator != null) animator.SetTrigger("Die");

        // Disable movement
        if (movement != null) movement.enabled = false;
        if (controller != null) controller.enabled = false;

        // Stop physics
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // Reset rotation
        transform.rotation = Quaternion.identity;

        // Only affect camera/look when this is the player (avoid killing global cameras when NPC dies)
        if (isPlayer)
        {
            // Disable CameraLook if explicitly assigned, otherwise try find one instance
            if (cameraLook != null)
            {
                cameraLook.isAlive = false;
            }
            else
            {
                var look = Object.FindObjectOfType<CameraLook>();
                if (look != null) look.isAlive = false;
            }

            // Switch cameras: only disable main if deathCamera is present
            if (deathCamera != null)
            {
                if (mainCamera != null) mainCamera.enabled = false;
                deathCamera.enabled = true;

                Vector3 camOffset = new Vector3(0, 10, -18);
                deathCamera.transform.position = transform.position + camOffset;
                Vector3 lookTarget = transform.position + new Vector3(0, 2, 0);
                deathCamera.transform.LookAt(lookTarget);
            }
            else
            {
                // If no death camera assigned, keep main camera active
                if (mainCamera != null) mainCamera.enabled = true;
                Debug.LogWarning("DeathCamera not assigned for player. Main camera remains active.");
            }
        }

        Debug.Log("Skeleton died: " + gameObject.name);
    }
}