using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class VFXDamageAfterDelay : MonoBehaviour
{
    [Tooltip("VFX prefab to spawn when this activates (optional)")]
    public GameObject vfxPrefab;

    [Tooltip("Delay in seconds after spawn before VFX and damage happen")]
    public float delay = 3f;

    [Tooltip("Damage amount applied by VFX to targets")]
    public int damage = 10;

    [Tooltip("VFX effective radius for damage (physics overlap)")]
    public float radius = 2f;

    [Tooltip("If set, only damage objects with this tag (leave empty to damage all)")]
    public string targetTag = "Skeleton";

    [Tooltip("Should the VFX be destroyed after spawning (true) or remain (false)")]
    public bool destroyVfxAfterSeconds = true;
    public float vfxLifetime = 5f;

    [Tooltip("If true, destroy the GameObject that contains this script when the spawned VFX is destroyed")]
    public bool destroyThisWithVfx = true;

    private void Start()
    {
        StartCoroutine(DoVfxAndDamage());
    }

    private IEnumerator DoVfxAndDamage()
    {
        yield return new WaitForSeconds(delay);

        // spawn VFX prefab if any
        if (vfxPrefab != null)
        {
            var v = Instantiate(vfxPrefab, transform.position, Quaternion.identity);
            if (destroyVfxAfterSeconds)
            {
                Destroy(v, vfxLifetime);
                if (destroyThisWithVfx)
                {
                    // destroy this prefab instance after the same lifetime so it is removed when VFX is removed
                    Destroy(gameObject, vfxLifetime);
                }
            }
        }
        else
        {
            // If there's no VFX prefab but the user wants this object removed after the delay, schedule destroy now
            if (destroyThisWithVfx)
            {
                Destroy(gameObject, vfxLifetime);
            }
        }

        // Apply damage via physics overlap
        Collider[] cols = Physics.OverlapSphere(transform.position, radius);
        foreach (var col in cols)
        {
            if (col == null) continue;
            var go = col.gameObject;
            if (!string.IsNullOrEmpty(targetTag) && !go.CompareTag(targetTag)) continue;

            var eh = go.GetComponent<EnemyHealth>() ?? go.GetComponentInParent<EnemyHealth>();
            if (eh != null)
            {
                eh.TakeDamage(damage);
            }
            else
            {
                go.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
