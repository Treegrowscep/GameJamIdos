using UnityEngine;

public class DragonController : MonoBehaviour
{
    public Animator animator;
    public Transform[] waypoints;
    public float flySpeed = 10f;
    public float waypointTolerance = 1f;

    private int currentWaypoint = 0;
    private bool isFlying = false;
    private bool isDead = false;

    void Start()
    {
        animator.SetTrigger("TakeOff");
        animator.SetBool("IsFlying", true);
        isFlying = true;
    }

    void Update()
    {
        if (isFlying && !isDead)
            FlyAlongPath();
    }

    void FlyAlongPath()
    {
        if (currentWaypoint >= waypoints.Length) return;

        Transform target = waypoints[currentWaypoint];
        Vector3 direction = (target.position - transform.position).normalized;

        transform.position += direction * flySpeed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 5f * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < waypointTolerance)
            currentWaypoint++;
    }

    public void Die()
    {
        isDead = true;
        animator.SetBool("IsDead", true);
        isFlying = false;
    }
}