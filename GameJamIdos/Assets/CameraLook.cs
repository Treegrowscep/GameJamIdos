using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    [Header("���������")]
    public Transform cameraTransform; // ������, ����������� �����/����
    public Transform playerBody;      // ���� �������, ����������� �����/������
    public float sensitivity = 2f;
    public float minVerticalAngle = -80f; // ����������� ����
    public float maxVerticalAngle = 80f;  // ����������� �����

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

        // ������� ������ �����/����
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // ������� ���� ������� �����/������
        playerBody.Rotate(Vector3.up * mouseX);
    }
}