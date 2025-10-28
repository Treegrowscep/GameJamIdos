using UnityEngine;
using UnityEngine.AI;

public class SkeletonAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private SkeletonTeam team;
    private GameObject currentTarget;
    private const float changeTargetDistance = 1.0f;

    [Header("Combat")]
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    public int damage = 10;
    public float moveSpeedFallback = 3f; // used if no NavMeshAgent present

    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // try several locations for team component
        team = GetComponent<SkeletonTeam>() ?? GetComponentInChildren<SkeletonTeam>() ?? GetComponentInParent<SkeletonTeam>();

        // Ensure agent is on navmesh if present
        if (agent != null && !agent.isOnNavMesh)
        {
            // try to sample a nearby navmesh position
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
            }
        }
    }

    void Update()
    {
        // allow AI to still function if there is no team (but log once)
        if (team == null)
        {
            // try recover team reference
            team = GetComponent<SkeletonTeam>() ?? GetComponentInChildren<SkeletonTeam>() ?? GetComponentInParent<SkeletonTeam>();
            if (team == null) return;
        }

        // Find target: enemy skeletons first, else generic objects
        GameObject target = FindNearestEnemySkeleton();
        if (target == null)
            target = FindNearestGenericTarget();

        if (target == null) return;

        float distToTarget = Vector3.Distance(transform.position, target.transform.position);

        // Move toward target: either via NavMeshAgent or fallback movement
        if (agent != null && agent.isOnNavMesh)
        {
            if (currentTarget != target || Vector3.Distance(agent.destination, target.transform.position) > changeTargetDistance)
            {
                currentTarget = target;
                agent.isStopped = false;
                agent.SetDestination(target.transform.position);
            }
        }
        else
        {
            // simple fallback: rotate and translate toward target
            Vector3 dir = (target.transform.position - transform.position);
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir.normalized), Time.deltaTime * 6f);
                transform.position += transform.forward * moveSpeedFallback * Time.deltaTime;
            }
        }

        // Attack when in range
        if (distToTarget <= attackRange)
        {
            if (agent != null && agent.isOnNavMesh) agent.isStopped = true;

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                var health = target.GetComponent<EnemyHealth>() ?? target.GetComponentInParent<EnemyHealth>();
                if (health != null && !health.IsDead)
                {
                    health.TakeDamage(damage);
                }
                else
                {
                    target.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
                }

                lastAttackTime = Time.time;
            }
        }
    }

    GameObject FindNearestEnemySkeleton()
    {
        float minDist = float.MaxValue;
        GameObject closest = null;

        var all = FindObjectsOfType<SkeletonTeam>();
        foreach (var other in all)
        {
            if (other == null || other.gameObject == gameObject) continue;
            if (team != null && other.teamID == team.teamID) continue; // only enemies
            float dist = Vector3.SqrMagnitude(other.transform.position - transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = other.gameObject;
            }
        }

        return closest;
    }

    GameObject FindNearestGenericTarget()
    {
        float minDist = float.MaxValue;
        GameObject closest = null;

        var healths = FindObjectsOfType<EnemyHealth>();
        foreach (var h in healths)
        {
            if (h == null || h.IsDead) continue;
            var st = h.GetComponent<SkeletonTeam>();
            if (st != null && team != null && st.teamID == team.teamID) continue; // skip same-team skeletons
            float dist = Vector3.SqrMagnitude(h.transform.position - transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = h.gameObject;
            }
        }

        if (closest != null) return closest;

        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var p in players)
        {
            float dist = Vector3.SqrMagnitude(p.transform.position - transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = p;
            }
        }

        if (closest != null) return closest;

        var targets = GameObject.FindGameObjectsWithTag("Target");
        foreach (var t in targets)
        {
            float dist = Vector3.SqrMagnitude(t.transform.position - transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = t;
            }
        }

        return closest;
    }
}