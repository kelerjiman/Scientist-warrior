using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class AnimationController
{
    private Animator m_Animator;
//    private Transform m_Target;
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int State = Animator.StringToHash("State");
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int Die = Animator.StringToHash("Die");
    public bool IsDie;

    public AnimationController(Animator anim)
    {
        m_Animator = anim;
//        m_Target = tar;
    }

    public void PlayMoveAnimation(AnimationClipType type)
    {
        if (IsDie)
            return;
        switch (type)
        {
            case AnimationClipType.Idle:
                m_Animator.SetInteger(State, 0);
                break;
            case AnimationClipType.Move:
                m_Animator.SetInteger(State, 1);
                break;
            default:
                m_Animator.SetInteger(State, 0);
                break;
        }
    }

    public void PlayAttackAnimation(bool value)
    {
            m_Animator.SetBool(Attack, value);
    }
    public void PlayDieAnimation()
    {
            m_Animator.SetTrigger(Die);
            IsDie = true;
    }

    public void PlayHitAnimation()
    {
        m_Animator.SetTrigger(Hit);
    }

//    public void Move(Vector2 target, int moveSpeed = 0)
//    {
//        var localScale = m_Target.localScale;
//        if (m_Target.position.x > target.x && Abs(Mathf.Sign(localScale.x) * 1 - 1) < 0.1)
//        {
//            localScale = new Vector2(-localScale.x, localScale.y);
//            m_Target.localScale = localScale;
//        }
//
//        if (m_Target.position.x < target.x && Abs(Mathf.Sign(localScale.x) * 1 - 1) > 0.1)
//        {
//            localScale = new Vector2(-localScale.x, localScale.y);
//            m_Target.localScale = localScale;
//        }
//
//        var position = m_Target.position;
//        position = Vector2.Lerp(position, target, moveSpeed * Time.deltaTime);
//        m_Target.position = position;
//        PlayAnimation(Abs(position.x - target.x) < 0.01f
//            ? AnimationClipType.Idle
//            : AnimationClipType.Move);
//    }
}

public enum AnimationClipType
{
    Idle,
    Move
}