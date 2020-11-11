using UnityEngine;
using Standard_Assets.CrossPlatformInput.Scripts;
using Script.InventorySystem;
using System;

public class PlayerAnimation : MonoBehaviour
{
    private static readonly int attackTypeState = Animator.StringToHash("AttackState");
    private Vector2 m_Direction = Vector2.zero;
    public Animator animator;
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Attack = Animator.StringToHash("Attack");
    public AttackType attackType;
    // Update is called once per frame
    private void Start()
    {
        HandAttackAnimation(attackType);
        InventoryManager.Instance.equipmentPanel.OnAddItemEvent += ChangeAttackAnimation_OnAddItemEvent;
        InventoryManager.Instance.equipmentPanel.OnRemoveItemEvent += ChangeAttackAnimation_OnRemoveEvent;
    }

    private void ChangeAttackAnimation_OnRemoveEvent(EquipableItem obj)
    {
        if (obj.BodyPartType == BodyPartType.Weapon)
        {
            attackType = AttackType.Spear;
        }
    }

    private void ChangeAttackAnimation_OnAddItemEvent(EquipableItem obj)
    {
        if (obj.BodyPartType == BodyPartType.Weapon)
        {
            attackType = ((Weapon)obj).attackType;
        }
    }

    void Update()
    {
        HandAttackAnimation(attackType);
        m_Direction.x = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        m_Direction.y = CrossPlatformInputManager.GetAxisRaw("Vertical");
        if (m_Direction.magnitude > 0)
            animator.SetInteger(Move, 1);
        else
            animator.SetInteger(Move, 0);
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
            animator.SetTrigger(Attack);

    }
    public void HandAttackAnimation(AttackType _attackType)
    {
        //if (attackType == _attackType)
        //    return;
        animator.SetInteger(attackTypeState, _attackType.GetHashCode());
        //attackType = _attackType;
    }

}
public enum AttackType
{
    Spear,
    Throwing,
    Hammer,
    Sword,
    Spell,
    Bow,
    Axe,
    TwoWeapon
}