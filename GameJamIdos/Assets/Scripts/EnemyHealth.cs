using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    [HideInInspector] public int currentHealth;

    [Header("Death settings")]
    public float destroyDelay = 5f;
    public bool spawnSkullOnDeath = true;

    // Event broadcast when an enemy dies (passes the GameObject that died)
    public static System.Action<GameObject> OnEnemyDied;

    private Animator animator;
    private NavMeshAgent agent;
    private Collider[] colliders;
    private MonoBehaviour[] disableOnDeath;
    private bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        colliders = GetComponentsInChildren<Collider>(true);

        var list = new System.Collections.Generic.List<MonoBehaviour>();
        var aiComp = GetComponent<SkeletonAI>();
        if (aiComp != null) list.Add(aiComp);
        var moveComp = GetComponent<SkeletonMovement>();
        if (moveComp != null) list.Add(moveComp);
        var combatComp = GetComponent<SkeletonCombat>();
        if (combatComp != null) list.Add(combatComp);
        var fightComp = GetComponent<SkeletonFight>();
        if (fightComp != null) list.Add(fightComp);

        disableOnDeath = list.ToArray();
    }

    private void OnValidate()
    {
        if (maxHealth < 1) maxHealth = 1;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (animator != null) animator.SetTrigger("Hit");
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null) animator.SetTrigger("Die");
        if (agent != null) agent.enabled = false;

        foreach (var col in colliders)
            if (col != null) col.enabled = false;

        foreach (var mb in disableOnDeath)
            if (mb != null) mb.enabled = false;

        var skull = GetComponent<SkullSpawner>();
        if (skull != null && spawnSkullOnDeath) skull.TriggerDeath();

        // Broadcast death before actual destroy so listeners can inspect the object
        OnEnemyDied?.Invoke(gameObject);

        Destroy(gameObject, destroyDelay);
    }

    public bool IsDead => isDead;
}