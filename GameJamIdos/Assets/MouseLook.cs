using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [Header("References")]
    public Transform playerBody;

    [Header("Settings")]
    public float sensitivity = 2f;
    public float minVerticalAngle = -80f;
    public float maxVerticalAngle = 0f; // ← запрещаем смотреть вверх

    private Vector2 lookInput;
    private float xRotation = 0f;
    private PlayerControls controls;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
    }

    void OnEnable()
    {
        controls.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Update()
    {
        float mouseX = lookInput.x * sensitivity * Time.deltaTime;
        float mouseY = lookInput.y * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);

        // Жёстко фиксируем вращение только по X (вниз или прямо)
        Vector3 currentEuler = transform.localEulerAngles;
        transform.localRotation = Quaternion.Euler(xRotation, currentEuler.y, currentEuler.z);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}