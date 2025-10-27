using UnityEngine;

public class SkullSpawner : MonoBehaviour
{
    [Header("Череп")]
    public GameObject skullDropPrefab; // Префаб черепа

    [Header("Автозапуск")]
    public bool spawnOnStart = false;

    private void Start()
    {
        if (spawnOnStart)
        {
            SpawnSkullAt(transform.position);
        }
    }

    public void TriggerDeath()
    {
        Debug.Log("☠️ Смерть наступила. Спавним череп...");
        SpawnSkullAt(transform.position);
    }

    private void SpawnSkullAt(Vector3 basePosition)
    {
        if (skullDropPrefab == null)
        {
            Debug.LogError("❌ Префаб черепа не назначен!");
            return;
        }

        Vector3 spawnPos = basePosition + Vector3.up * 1.5f;

        RaycastHit hit;
        if (Physics.Raycast(spawnPos, Vector3.down, out hit, 10f))
        {
            spawnPos = hit.point + Vector3.up * 1f;
            Debug.Log("✅ Земля найдена: " + hit.collider.name);
        }
        else
        {
            Debug.LogWarning("⚠️ Земля не найдена — спавним над телом");
        }

        GameObject skull = Instantiate(skullDropPrefab, spawnPos, Quaternion.identity);
        skull.SetActive(true);
        Debug.Log("💀 Череп создан над телом: " + skull.name);
    }
}
