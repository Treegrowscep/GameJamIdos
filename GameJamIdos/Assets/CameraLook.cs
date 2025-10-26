using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    [Header("Настройки")]
    public Transform cameraTransform; // Камера, вращающаяся вверх/вниз
    public Transform playerBody;      // Тело скелета, вращающееся влево/вправо
    public float sensitivity = 2f;
    public float minVerticalAngle = -80f; // Ограничение вниз
    public float maxVerticalAngle = 80f;  // Ограничение вверх

    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnLook(InputValue value)
    {
        Vector2 lookDelta = value.Get<Vector2>();
        float mouseX = lookDelta.x * sensitivity;
        float mouseY = lookDelta.y * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);

        // Вращаем камеру вверх/вниз
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Вращаем тело скелета влево/вправо
        playerBody.Rotate(Vector3.up * mouseX);
    }
}