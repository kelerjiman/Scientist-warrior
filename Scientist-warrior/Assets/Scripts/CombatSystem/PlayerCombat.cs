using System;
using Script;
using Script.CharacterStatus;
using Script.CombatSystem;
using Script.InventorySystem;
using Standard_Assets.CrossPlatformInput.Scripts;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace CombatSystem
{
    public class PlayerCombat : CombatBase
    {
        private Collider2D[] _mTragets;
        private State _mAttackState, _mMaxHealth, _mMaxEnergy;
        [SerializeField] private SliderScript healthSlider;
        [SerializeField] private SliderScript energySlider;
        [SerializeField] private TextMeshProUGUI attackAmountText;
        [SerializeField] private Transform ProjectilePlaceHolder;
        [SerializeField] private ParicalScript Partical;
        [SerializeField] private GameObject playerVisual;
        InventoryManager _inventoryManager;
        Projectile _currentProjectile;
        private void Start()
        {

            _inventoryManager = InventoryManager.Instance;
            _inventoryManager.equipmentPanel.OnAddItemEvent += GetWeapon_OnAddItemEvent;
            _mAttackState = StateManager.Instance.characterState.states.Find(state => state.type == StateType.Damage);
            _mMaxHealth = StateManager.Instance.characterState.states.Find(state => state.type == StateType.MaxHealth);
            _mMaxEnergy = StateManager.Instance.characterState.states.Find(state => state.type == StateType.MaxEnergy);
            CurrentChange();
        }

        private void GetWeapon_OnAddItemEvent(EquipableItem obj)
        {
            Weapon target = obj as Weapon;
            if (target == null)
                return;
            if (target.weaponType == Weapon.WeaponType.Bow || target.weaponType == Weapon.WeaponType.Staff)
                _currentProjectile = target.projectile;
            else
                _currentProjectile = null;
        }

        private void Update()
        {
            
            if(!GameManager.Instance.gameContinue)
                return;
            if (CrossPlatformInputManager.GetButtonDown("Fire1"))
            {
                Attack();
            }

            attackAmountText.text = _mAttackState.amount.ToString();
        }
        void Attack()
        {
            if (_currentProjectile == null)
            {
                _mTragets = Physics2D.OverlapCircleAll(AttackPoint.position, Radius, TargetLayer);
                foreach (var target in _mTragets)
                {
                    //Debug.Log("this is the target" + target.name);
                    target.GetComponent<IINteractable>().GetDamage((int)_mAttackState.amount);
                }
            }
            else
            {
                var projectile = Instantiate(_currentProjectile, ProjectilePlaceHolder.position, quaternion.identity);
                projectile.damage += (int)StateManager.Instance.characterState.states.Find(s => s.type == StateType.Damage).amount;
                if (Math.Abs(Mathf.Sign(projectile.velocity.x) - Mathf.Sign(transform.localScale.x)) > 0.1f)
                {
                    var transform1 = projectile.transform;
                    var localScale = transform1.localScale;
                    localScale.x *= -1;
                    transform1.localScale = localScale;
                    projectile.velocity.x *= -1;
                }
            }
        }


        public void RefreshSliders()
        {
            if (CurrentHealth <= 0)
            {
                GameManager.Instance.gameContinue = false;
                playerVisual.SetActive(false);
                gameObject.SetActive(false);
                Instantiate(Partical, playerVisual.transform.position, quaternion.identity);
            }   
            healthSlider.MaxAmount = _mMaxHealth.amount;
            energySlider.MaxAmount = _mMaxEnergy.amount;
            healthSlider.CurrentAmount = CurrentHealth;
            energySlider.CurrentAmount = CurrentEnergy;
        }

        public void CurrentChange(int energy = 0, int health = 0)
        {
            CurrentHealth += health;
            CurrentEnergy += energy;
            if (CurrentHealth > _mMaxHealth.amount)
                CurrentHealth = (int)_mMaxHealth.amount;
            if (CurrentEnergy > _mMaxEnergy.amount)
                CurrentEnergy = (int)_mMaxEnergy.amount;
            RefreshSliders();
        }

        public override void GetDamage(int damage)
        {
            base.GetDamage(damage);
            RefreshSliders();
        }

        private void OnDrawGizmosSelected()
        {
            if (AttackPoint == null)
            {
                Debug.LogWarning("Set Attack Point please");
                return;
            }

            Gizmos.DrawWireSphere(AttackPoint.position, Radius);
        }
    }
}