using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SkeletonHealth : MonoBehaviour
{
    public Image heartIcon;
    public Camera mainCamera;
    public Camera deathCamera;

    private bool isAlive = true;

    private void Update()
    {
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        if (!isAlive) return;
        isAlive = false;

        // Отключаем сердце
        if (heartIcon != null)
            heartIcon.enabled = false;

        // Анимация смерти
        Animator animator = GetComponent<Animator>();
        if (animator != null)
            animator.SetTrigger("Die");

        // Отключаем движение
        var movement = GetComponent<SkeletonMovement>();
        if (movement != null) movement.enabled = false;

        var controller = GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // Принудительно сбрасываем поворот
        transform.rotation = Quaternion.identity;

        // Отключаем CameraLook на активной камере
        var lookScript = FindObjectOfType<CameraLook>();
        if (lookScript != null)
            lookScript.isAlive = false;

        // Ставим DeathCamera чуть ближе, но с обзором
        if (deathCamera != null)
        {
            Vector3 camOffset = new Vector3(0, 10, -18);
            deathCamera.transform.position = transform.position + camOffset;

            Vector3 lookTarget = transform.position + new Vector3(0, 2, 0);
            deathCamera.transform.LookAt(lookTarget);
        }

        // Переключаем камеры
        if (mainCamera != null) mainCamera.enabled = false;
        if (deathCamera != null) deathCamera.enabled = true;

        Debug.Log("Skeleton died");
    }
}