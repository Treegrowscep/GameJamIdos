using UnityEngine;

public class DragonSpawner : MonoBehaviour
{
    public GameObject dragonPrefab;
    public Transform spawnPoint;
    public Transform[] pathWaypoints;

    private bool hasSpawned = false;

    void Start()
    {
        if (hasSpawned) return;

        GameObject dragon = Instantiate(dragonPrefab, spawnPoint.position, spawnPoint.rotation);
        DragonController controller = dragon.GetComponent<DragonController>();
        controller.waypoints = pathWaypoints;

        hasSpawned = true;
    }
}