using UnityEngine;
using UnityEngine.AI;

public class SkeletonAI : MonoBehaviour
{
    public float detectionRadius = 30f;
    public float updateRate = 0.5f; // реже обновляем путь — меньше рывков

    private NavMeshAgent agent;
    private SkeletonTeam team;
    private Animator anim;
    private float timer;

    private Transform currentTarget; // ✅ сохраняем цель
    private Vector3 lastTargetPos;   // ✅ последняя позиция цели

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        team = GetComponent<SkeletonTeam>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateRate)
        {
            timer = 0f;
            FindTarget();
            UpdatePath();
        }

        UpdateAnimation();
    }

    // ✅ Ищем ближайшего врага, но НЕ обновляем путь здесь
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

        if (nearest != null)
        {
            currentTarget = nearest;
        }
    }

    // ✅ Обновляем путь ТОЛЬКО если цель реально сместилась
    void UpdatePath()
    {
        if (currentTarget == null) return;

        float distMoved = Vector3.Distance(lastTargetPos, currentTarget.position);

        if (distMoved > 1f) // ✅ обновляем путь только если цель сместилась > 1 метра
        {
            agent.isStopped = false;
            agent.SetDestination(currentTarget.position);
            lastTargetPos = currentTarget.position;
        }
    }

    // ✅ Плавная анимация
    void UpdateAnimation()
    {
        if (anim == null || agent == null)
            return;

        // нормализуем скорость
        float normalized = agent.velocity.magnitude / agent.speed;
        normalized = Mathf.Clamp01(normalized);

        // масштабируем под Blend Tree (Idle=0, Walk=1, Run=3)
        float targetSpeed = normalized * 3f;

        // сглаживаем
        float smoothSpeed = Mathf.Lerp(anim.GetFloat("Speed"), targetSpeed, Time.deltaTime * 8f);

        anim.SetFloat("Speed", smoothSpeed);
    }
}