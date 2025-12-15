using UnityEngine;

public class SkeletonSpawner : MonoBehaviour
{
    public GameObject SkeletonPrefab;
    public Transform[] SpawnPoints;
    public int SkeletonsPerTeam = 3;
    public float spawnSpread = 2f; // радиус разброса

    void Start()
    {
        if (SkeletonPrefab == null)
        {
            Debug.LogError("❌ Skeleton Prefab не назначен!");
            return;
        }

        if (SpawnPoints == null || SpawnPoints.Length == 0)
        {
            Debug.LogError("❌ Нет точек спавна!");
            return;
        }

        Debug.Log($"✅ Спавню {SkeletonsPerTeam} скелетов...");

        for (int i = 0; i < SkeletonsPerTeam; i++)
        {
            int index = Random.Range(0, SpawnPoints.Length);
            Transform point = SpawnPoints[index];

            Vector3 offset = new Vector3(
                Random.Range(-spawnSpread, spawnSpread),
                0f,
                Random.Range(-spawnSpread, spawnSpread)
            );

            Vector3 spawnPos = point.position + offset;

            GameObject skeleton = Instantiate(SkeletonPrefab, spawnPos, point.rotation);
            Debug.Log($"☠️ Скелет {i + 1} заспавнен в {spawnPos}");
        }
    }
}