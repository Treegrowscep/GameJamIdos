using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class SpawnedSkeleton : MonoBehaviour
{
    [HideInInspector] public SkeletonSpawner spawner;

    private void Awake()
    {
        // Ensure this object has health — registration is done by spawner when instantiated
    }

    private void OnEnable()
    {
        // optionally notify spawner
        if (spawner != null) spawner.RegisterSpawn(gameObject);
    }

    private void OnDestroy()
    {
        // Notify spawner to respawn
        if (spawner != null) spawner.UnregisterSpawn(gameObject);
    }
}
