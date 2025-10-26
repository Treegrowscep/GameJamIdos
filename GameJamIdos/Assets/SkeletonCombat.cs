using UnityEngine;
using UnityEngine.InputSystem;

public class SkeletonCombat : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnAttackPrimary(InputValue value)
    {
        if (value.isPressed)
        {
            animator.SetTrigger("Attack1");
        }
    }

    public void OnAttackSecondary(InputValue value)
    {
        if (value.isPressed)
        {
            animator.SetTrigger("Attack2");
        }
    }
}
