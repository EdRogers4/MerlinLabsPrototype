using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void TogglePlayerAttackOff()
    {
        animator.SetBool("isAttack", false);
    }

    public void ToggleEnemyTakeDamageOff()
    {
        animator.SetBool("isTakeDamage", false);
    }
}
