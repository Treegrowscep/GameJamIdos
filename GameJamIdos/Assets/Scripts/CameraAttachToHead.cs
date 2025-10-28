using UnityEngine;

public class CameraAttachToHead : MonoBehaviour
{
    public Transform headTransform; // сюда назначь голову скелета
    public Vector3 localOffset = new Vector3(0, 0.2f, -0.5f); // чуть выше и позади головы

    void LateUpdate()
    {
        if (headTransform == null) return;

        // Камера следует за головой с небольшим смещением
        transform.position = headTransform.position + headTransform.TransformDirection(localOffset);
        transform.rotation = headTransform.rotation;
    }
}