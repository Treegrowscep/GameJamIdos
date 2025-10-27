using UnityEngine;
using UnityEngine.AI;

public class SkeletonAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private SkeletonTeam team;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        team = GetComponent<SkeletonTeam>();
    }

    void Update()
    {
        GameObject target = FindNearestEnemy();
        if (target != null)
            agent.SetDestination(target.transform.position);
    }

    GameObject FindNearestEnemy()
    {
        float minDist = Mathf.Infinity;
        GameObject closest = null;

        foreach (var skeleton in GameObject.FindGameObjectsWithTag("Skeleton"))
        {
            if (skeleton == gameObject) continue;

            SkeletonTeam otherTeam = skeleton.GetComponent<SkeletonTeam>();
            if (otherTeam != null && otherTeam.teamID != team.teamID)
            {
                float dist = Vector3.Distance(transform.position, skeleton.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = skeleton;
                }
            }
        }

        return closest;
    }
}