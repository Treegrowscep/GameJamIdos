using System.Collections.Generic;
using UnityEngine;

[HelpURL("https://example.com/docs/damageontouch")]
public class DamageOnTouch : MonoBehaviour
{
    [Tooltip("Amount of damage to apply")]
    public int damage = 10;

    [Tooltip("If set, only objects with this tag will be damaged. Leave empty to damage any object.")]
    public string targetTag = "Skeleton";

    [Tooltip("Use trigger events (collider must be " + "isTrigger) or collision events")]
    public bool useTrigger = true;

    [Tooltip("Apply damage immediately on enter")]
    public bool damageOnEnter = true;

    [Tooltip("Apply damage repeatedly while staying in contact")]
    public bool damageOnStay = false;

    [Tooltip("Minimum seconds between repeated damage applications per target")]
    public float damageInterval = 1f;

    // Track last damage time per target to implement interval
    private readonly Dictionary<GameObject, float> lastDamageTime = new Dictionary<GameObject, float>();

    private void Reset()
    {
        // Ensure there is a Collider and set its trigger mode to match useTrigger
        var col = GetComponent<Collider>();
        if (col == null)
        {
            var box = gameObject.AddComponent<BoxCollider>();
            box.isTrigger = useTrigger;
        }
        else
        {
            col.isTrigger = useTrigger;
        }

        // If nothing specified for tag, leave empty to damage any target
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!useTrigger) return;
        if (!damageOnEnter) return;
        TryDamage(other.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!useTrigger) return;
        if (!damageOnStay) return;
        TryDamageWithInterval(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (useTrigger) return;
        if (!damageOnEnter) return;
        TryDamage(collision.gameObject);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (useTrigger) return;
        if (!damageOnStay) return;
        TryDamageWithInterval(collision.gameObject);
    }

    private void TryDamageWithInterval(GameObject target)
    {
        if (!IsValidTarget(target)) return;

        float last;
        if (lastDamageTime.TryGetValue(target, out last))
        {
            if (Time.time - last < damageInterval) return;
        }

        ApplyDamageToTarget(target);
        lastDamageTime[target] = Time.time;
    }

    private void TryDamage(GameObject target)
    {
        if (!IsValidTarget(target)) return;
        ApplyDamageToTarget(target);
        lastDamageTime[target] = Time.time;
    }

    private bool IsValidTarget(GameObject target)
    {
        if (target == null) return false;
        if (target == gameObject) return false;
        if (!string.IsNullOrEmpty(targetTag) && !target.CompareTag(targetTag)) return false;
        return true;
    }

    private void ApplyDamageToTarget(GameObject target)
    {
        if (target == null) return;

        // Try common health components on the target itself
        var enemy = target.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            return;
        }

        var skeletonWrapper = target.GetComponent<SkeletonHP_Death>();
        if (skeletonWrapper != null)
        {
            skeletonWrapper.ApplyDamage(damage);
            return;
        }

        // Also try parent hierarchy (in case health is on parent)
        enemy = target.GetComponentInParent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            return;
        }

        skeletonWrapper = target.GetComponentInParent<SkeletonHP_Death>();
        if (skeletonWrapper != null)
        {
            skeletonWrapper.ApplyDamage(damage);
            return;
        }

        // As a last resort, try to call a method named "TakeDamage" via SendMessage (safe - no exception if not present)
        target.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
    }
}