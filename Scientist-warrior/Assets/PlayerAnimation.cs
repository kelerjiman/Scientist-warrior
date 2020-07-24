using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerAnimation : MonoBehaviour
{
    private Vector2 m_Direction = Vector2.zero;
    public Animator animator;
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Attack = Animator.StringToHash("Attack");

    // Update is called once per frame
    void Update()
    {
        m_Direction.x = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        m_Direction.y = CrossPlatformInputManager.GetAxisRaw("Vertical");
        if (m_Direction.magnitude > 0)
            animator.SetInteger(Move, 1);
        else
            animator.SetInteger(Move, 0);
        if(CrossPlatformInputManager.GetButtonDown("Fire1"))
            animator.SetTrigger(Attack);
    }
}