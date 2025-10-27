using UnityEngine;

public class SkeletonSpawner : MonoBehaviour
{
    public GameObject skeletonPrefab;
    public Transform[] spawnPoints;
    public int skeletonsPerTeam = 3;

    void Start()
    {
        SpawnTeam(0); // команда 0
        SpawnTeam(1); // команда 1
    }

    void SpawnTeam(int teamID)
    {
        for (int i = 0; i < skeletonsPerTeam; i++)
        {
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject skeleton = Instantiate(skeletonPrefab, spawn.position, spawn.rotation);
            skeleton.GetComponent<SkeletonTeam>().teamID = teamID;
        }
    }
}