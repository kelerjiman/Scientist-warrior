using UnityEngine;

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
}

public enum AnimationClipType
{
    Idle,
    Move
}