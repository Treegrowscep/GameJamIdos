using UnityEngine;

public class CameraFollowSkelet : MonoBehaviour
{
    public Transform skeleton; // Назначь сюда объект скелета (не голову)
    public Vector3 offset = new Vector3(0, 1.6f, -2f); // Высота и отдаление
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (skeleton == null) return;

        Vector3 desiredPosition = skeleton.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(skeleton); // Камера всегда смотрит на скелета
    }
}