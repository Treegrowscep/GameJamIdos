using UnityEngine;

public class SkeletonFight : MonoBehaviour
{
    [Header("Combat Settings")]
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    public int damage = 10;

    private float lastAttackTime;
    private SkeletonTeam team;
    private Collider[] overlapBuffer = new Collider[16];

    void Start()
    {
        team = GetComponent<SkeletonTeam>();
    }

    void Update()
    {
        if (team == null) return;

        int hits = Physics.OverlapSphereNonAlloc(transform.position, attackRange, overlapBuffer);
        for (int i = 0; i < hits; i++)
        {
            var hit = overlapBuffer[i];
            if (hit == null || hit.gameObject == gameObject) continue;

            var otherTeam = hit.GetComponent<SkeletonTeam>();
            if (otherTeam != null && otherTeam.teamID != team.teamID)
            {
                if (Time.time - lastAttackTime > attackCooldown)
                {
                    var health = hit.GetComponent<EnemyHealth>();
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