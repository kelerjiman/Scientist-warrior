using UnityEngine;
using Standard_Assets.CrossPlatformInput.Scripts;
using Script.InventorySystem;
using System;

public class PlayerAnimation : MonoBehaviour
{
    private static readonly int attackTypeState = Animator.StringToHash("AttackState");
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private Vector2 m_Direction = Vector2.zero;
    private AttackType defaultAttackType;
    private Animator animator;

    public AttackType attackType;
    [SerializeField] private GameObject AnimatorHolder;


    // Update is called once per frame
    private void Start()
    {
        animator = AnimatorHolder.GetComponent<Animator>();
        defaultAttackType = attackType;
        HandAttackAnimation(attackType);
        InventoryManager.Instance.equipmentPanel.OnAddItemEvent += ChangeAttackAnimation_OnAddItemEvent;
        InventoryManager.Instance.equipmentPanel.OnRemoveItemEvent += ChangeAttackAnimation_OnRemoveEvent;
    }

    private void ChangeAttackAnimation_OnRemoveEvent(EquipableItem obj)
    {
        Weapon temp;
        var targetWeapon = obj as Weapon;
        if (targetWeapon != null)
        {
            if (targetWeapon.EquipmentType == EquipmentType.MainHand)
                temp = InventoryManager.Instance.equipmentPanel.GetSlot(EquipmentType.Shield).Item as Weapon;
            else
                temp = InventoryManager.Instance.equipmentPanel.GetSlot(EquipmentType.MainHand).Item as Weapon;

            if (temp != null)
                attackType = temp.attackType ;
            else
                attackType = defaultAttackType;
        }
        //Debug.Log("Change Attack animation OnRemoveEvent");
    }

    private void ChangeAttackAnimation_OnAddItemEvent(EquipableItem obj)
    {
        Weapon temp;
        var targetWeapon = obj as Weapon;
        if (targetWeapon != null)
        {
            if (targetWeapon.EquipmentType == EquipmentType.MainHand)
                temp = InventoryManager.Instance.equipmentPanel.GetSlot(EquipmentType.Shield).Item as Weapon;
            else
                temp = InventoryManager.Instance.equipmentPanel.GetSlot(EquipmentType.MainHand).Item as Weapon;

            if (temp != null)
                attackType = AttackType.TwoWeapon;
            else
                attackType = targetWeapon.attackType;
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