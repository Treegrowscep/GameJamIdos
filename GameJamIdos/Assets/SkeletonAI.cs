using UnityEngine;
using UnityEngine.AI;

public class SkeletonAI : MonoBehaviour
{
    public float detectionRadius = 30f;
    public float updateRate = 0.3f;

    private NavMeshAgent agent;
    private SkeletonTeam team;
    private Animator anim;

    private Transform currentTarget;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        team = GetComponent<SkeletonTeam>();
        anim = GetComponent<Animator>();

        // ✅ Плавное движение
        agent.autoBraking = false;

        // ✅ Улучшенное избегание столкновений
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        agent.avoidancePriority = Random.Range(20, 80);

        // ✅ Моментальный старт
        FindTarget();
        UpdateDestination();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateRate)
        {
            timer = 0f;
            FindTarget();
            UpdateDestination();
        }

        UpdateAnimation();
    }

    void FindTarget()
    {
        GameObject[] all = GameObject.FindGameObjectsWithTag("Skeleton");

        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject obj in all)
        {
            if (obj == this.gameObject) continue;

            SkeletonTeam otherTeam = obj.GetComponent<SkeletonTeam>();
            if (otherTeam == null) continue;

            if (otherTeam.teamID == team.teamID) continue;

            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if (dist < minDist && dist <= detectionRadius)
            {
                minDist = dist;
                nearest = obj.transform;
            }
        }

        currentTarget = nearest;
    }

    void UpdateDestination()
    {
        if (currentTarget == null) return;

        // ✅ РАССЫПАНИЕ — каждый скелет бежит в свою точку вокруг врага
        Vector3 offset = Random.insideUnitSphere * 1.5f;
        offset.y = 0;

        agent.isStopped = false;
        agent.SetDestination(currentTarget.position + offset);
    }

    void UpdateAnimation()
    {
        if (anim == null || agent == null)
            return;

        // ✅ desiredVelocity — НЕ скачет, в отличие от velocity
        float desired = agent.desiredVelocity.magnitude;

        // ✅ нормализуем 0–1
        float normalized = Mathf.Clamp01(desired / agent.speed);

        // ✅ масштаб под Blend Tree (Idle=0, Walk=1, Run=3)
        float targetSpeed = normalized * 3f;

        // ✅ сглаживание
        float smooth = Mathf.Lerp(anim.GetFloat("Speed"), targetSpeed, Time.deltaTime * 10f);

        anim.SetFloat("Speed", smooth);
    }
}