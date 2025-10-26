using UnityEngine;

public class SkeletonMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotateSpeed = 720f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>(); // Получаем компонент Animator
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(h, 0, v).normalized;
        float speed = direction.magnitude * moveSpeed;

        // Передаём значение в Animator
        animator.SetFloat("Speed", speed);

        if (speed > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
        }
    }
}