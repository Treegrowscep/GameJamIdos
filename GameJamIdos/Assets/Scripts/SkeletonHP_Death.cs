using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class SkeletonHP_Death : MonoBehaviour
{
    private EnemyHealth hp;
    private bool deathHandled = false;
    private Animator animator;

    private void Awake()
    {
        hp = GetComponent<EnemyHealth>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (hp == null) return;

        if (!deathHandled && hp.IsDead)
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        deathHandled = true;

        // Play death animation if available
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Disable common movement / AI / combat scripts if present
        var movement = GetComponent<SkeletonMovement>();
        if (movement != null) movement.enabled = false;

        var ai = GetComponent<SkeletonAI>();
        if (ai != null) ai.enabled = false;

        var combat = GetComponent<SkeletonCombat>();
        if (combat != null) combat.enabled = false;

        var fight = GetComponent<SkeletonFight>();
        if (fight != null) fight.enabled = false;

        // Disable character controller if present
        var controller = GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;

        // Stop and (optionally) kinematic rigidbody to avoid physics jank
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // Do not modify global cameras here � camera switching should be handled by player/camera manager.

        Debug.Log(gameObject.name + " died (SkeletonHP_Death handled)");
    }

    // Expose a helper to apply damage (can be called by other systems)
    public void ApplyDamage(int amount)
    {
        if (hp == null) return;
        hp.TakeDamage(amount);
    }

    // Optional: UI hook or debug
    private void OnGUI()
    {
        if (hp == null) return;
        if (Camera.main == null) return; // guard against no camera

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2f);
        if (screenPos.z > 0)
        {
            Vector2 size = new Vector2(60, 6);
            Rect rect = new Rect(screenPos.x - size.x / 2f, Screen.height - screenPos.y, size.x, size.y);
            float ratio = Mathf.Clamp01((float)hp.currentHealth / hp.maxHealth);
            // Background
            GUI.color = Color.black;
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            // Foreground
            rect.width *= ratio;
            GUI.color = Color.green;
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = Color.white;
        }
    }
}