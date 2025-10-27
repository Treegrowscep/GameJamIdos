using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    [Header("Настройки")]
    public Transform cameraTransform;     // Камера, вращающаяся вверх/вниз
    public Transform playerBody;          // Тело скелета (вращается влево/вправо)
    public float sensitivity = 1f;
    public float minVerticalAngle = -60f;
    public float maxVerticalAngle = 60f;

    [Header("Состояние")]
    public bool isAlive = true;           // Управление разрешено только если скелет жив

    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnLook(InputValue value)
    {
        if (!isAlive) return;

        Vector2 lookDelta = value.Get<Vector2>();

        float mouseX = lookDelta.x * sensitivity;
        float mouseY = lookDelta.y * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}