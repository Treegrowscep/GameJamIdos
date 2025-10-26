using UnityEngine;

public class CameraFollowSkelet : MonoBehaviour
{
    public Transform skeleton; // ������� ���� ������ ������� (�� ������)
    public Vector3 offset = new Vector3(0, 1.6f, -2f); // ������ � ���������
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (skeleton == null) return;

        Vector3 desiredPosition = skeleton.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(skeleton); // ������ ������ ������� �� �������
    }
}