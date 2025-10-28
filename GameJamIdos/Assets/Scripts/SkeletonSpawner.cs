using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSpawner : MonoBehaviour
{
    public GameObject skeletonPrefab;
    public Transform[] spawnPoints;
    public int skeletonsPerTeam = 3;

    // respawn delay in seconds
    public float respawnDelay = 2f;

    // track active instances spawned by this spawner
    private readonly List<GameObject> activeSpawned = new List<GameObject>();

    void Start()
    {
        SpawnTeam(0);
        SpawnTeam(1);
    }

    void SpawnTeam(int teamID)
    {
        if (skeletonPrefab == null)
        {
            Debug.LogWarning("SkeletonSpawner: skeletonPrefab is null.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("SkeletonSpawner: no spawn points assigned.");
            return;
        }

        for (int i = 0; i < skeletonsPerTeam; i++)
        {
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject skeleton = Instantiate(skeletonPrefab, spawn.position, spawn.rotation);

            // Ensure a SkeletonTeam exists and assign teamID
            var teamComp = skeleton.GetComponent<SkeletonTeam>();
            if (teamComp == null) teamComp = skeleton.AddComponent<SkeletonTeam>();
            teamComp.teamID = teamID;

            // Ensure health so attacks apply
            var health = skeleton.GetComponent<EnemyHealth>();
            if (health == null) skeleton.AddComponent<EnemyHealth>();

            // Attach helper to notify spawner when this instance is destroyed
            var helper = skeleton.GetComponent<SpawnedSkeleton>();
            if (helper == null) helper = skeleton.AddComponent<SpawnedSkeleton>();
            helper.spawner = this;

            RegisterSpawn(skeleton);
        }
    }

    // Called by SpawnedSkeleton when instance is enabled/created
    public void RegisterSpawn(GameObject instance)
    {
        if (instance == null) return;
        if (!activeSpawned.Contains(instance)) activeSpawned.Add(instance);
    }

    // Called by SpawnedSkeleton when instance is destroyed
    public void UnregisterSpawn(GameObject instance)
    {
        if (instance == null) return;
        // preserve team id if possible
        int teamID = -1;
        var team = instance.GetComponent<SkeletonTeam>();
        if (team != null) teamID = team.teamID;

        activeSpawned.Remove(instance);
        StartCoroutine(RespawnReplacementAfterDelay(teamID, respawnDelay));
    }

    private IEnumerator RespawnReplacementAfterDelay(int teamID, float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnReplacement(teamID);
    }

    private void SpawnReplacement(int teamID)
    {
        if (skeletonPrefab == null) return;
        if (spawnPoints == null || spawnPoints.Length == 0) return;

        // choose random index among first three spawn points if available
        int maxChoices = Mathf.Min(3, spawnPoints.Length);
        int idx = Random.Range(0, maxChoices);
        Transform spawn = spawnPoints[idx];

        GameObject skeleton = Instantiate(skeletonPrefab, spawn.position, spawn.rotation);

        var teamComp = skeleton.GetComponent<SkeletonTeam>();
        if (teamComp == null) teamComp = skeleton.AddComponent<SkeletonTeam>();
        if (teamID >= 0) teamComp.teamID = teamID;

        var helper = skeleton.GetComponent<SpawnedSkeleton>();
        if (helper == null) helper = skeleton.AddComponent<SpawnedSkeleton>();
        helper.spawner = this;

        RegisterSpawn(skeleton);
    }
}