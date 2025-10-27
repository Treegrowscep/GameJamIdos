using UnityEngine;
using UnityEngine.AI;

public class SkeletonController : MonoBehaviour
{
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    public int damage = 10;

    private NavMeshAgent agent;
    private Animator animator;
    private SkeletonTeam team;
    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        team = GetComponent<SkeletonTeam>();
    }

    void Update()
    {
        GameObject target = FindNearestEnemy();
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            agent.SetDestination(target.transform.position);

            animator.SetFloat("Speed", agent.velocity.magnitude);

            if (distance <= attackRange && Time.time - lastAttackTime > attackCooldown)
            {
                animator.SetTrigger("Attack");
                lastAttackTime = Time.time;

                var health = target.GetComponent<EnemyHealth>();
                if (health != null)
                    health.TakeDamage(damage);
            }
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] all = GameObject.FindGameObjectsWithTag("Skeleton");
        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject obj in all)
        {
            if (obj == gameObject) continue;

            SkeletonTeam otherTeam = obj.GetComponent<SkeletonTeam>();
            if (otherTeam != null && otherTeam.teamID != team.teamID)
            {
                float dist = Vector3.Distance(transform.position, obj.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = obj;
                }
            }
        }

        return closest;
    }
}