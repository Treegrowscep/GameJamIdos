using UnityEngine;

public class CameraAttachToHead : MonoBehaviour
{
    public Transform headTransform; // ���� ������� ������ �������
    public Vector3 localOffset = new Vector3(0, 0.2f, -0.5f); // ���� ���� � ������ ������

    void LateUpdate()
    {
        if (headTransform == null) return;

        // ������ ������� �� ������� � ��������� ���������
        transform.position = headTransform.position + headTransform.TransformDirection(localOffset);
        transform.rotation = headTransform.rotation;
    }
}