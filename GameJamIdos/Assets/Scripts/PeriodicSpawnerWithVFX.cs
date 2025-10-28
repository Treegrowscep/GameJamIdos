using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class PeriodicSpawnerWithVFX : MonoBehaviour
{
    [Tooltip("Prefab to spawn every interval. The prefab should contain a VfxDamageAfterDelay script or similar behaviour.")]
    public GameObject spawnPrefab;

    [Tooltip("Where to spawn. If null, this object's transform will be used.")]
    public Transform spawnPoint;

    [Tooltip("Seconds between spawns")]
    public float spawnInterval = 20f;

    [Tooltip("Spawn immediately on Start if true")]
    public bool spawnOnStart = true;

    private Coroutine spawnRoutine;

    private void OnValidate()
    {
        if (spawnInterval < 0.1f) spawnInterval = 0.1f;
    }

    private void Start()
    {
        if (spawnOnStart)
        {
            SpawnOne();
        }

        spawnRoutine = StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnOne();
        }
    }

    private void SpawnOne()
    {
        if (spawnPrefab == null)
        {
            Debug.LogWarning("PeriodicSpawnerWithVfx: spawnPrefab is not assigned.", this);
            return;
        }

        Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position;
        Quaternion rot = spawnPoint != null ? spawnPoint.rotation : transform.rotation;

        GameObject go = Instantiate(spawnPrefab, pos, rot);
        // optional: name for clarity
        go.name = spawnPrefab.name + "_" + Time.frameCount;
    }

    private void OnDisable()
    {
        if (spawnRoutine != null) StopCoroutine(spawnRoutine);
    }
}
