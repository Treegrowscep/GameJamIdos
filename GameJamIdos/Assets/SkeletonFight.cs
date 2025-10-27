using UnityEngine;

public class SkeletonFight : MonoBehaviour
{
    [Header("Combat Settings")]
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    public int damage = 10;

    private float lastAttackTime;
    private SkeletonTeam team;

    void Start()
    {
        team = GetComponent<SkeletonTeam>();
    }

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange);
        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject) continue;

            SkeletonTeam otherTeam = hit.GetComponent<SkeletonTeam>();
            if (otherTeam != null && otherTeam.teamID != team.teamID)
            {
                if (Time.time - lastAttackTime > attackCooldown)
                {
                    EnemyHealth health = hit.GetComponent<EnemyHealth>();
                    if (health != null)
                    {
                        health.TakeDamage(damage);
                        lastAttackTime = Time.time;
                    }
                }
            }
        }
    }
}