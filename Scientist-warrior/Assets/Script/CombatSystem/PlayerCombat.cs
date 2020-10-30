using Script.CharacterStatus;
using Script.InventorySystem;
using Unity.Mathematics;
using UnityEngine;
using Standard_Assets.CrossPlatformInput.Scripts;

namespace Script.CombatSystem
{
    public class PlayerCombat : CombatBase
    {
        private Collider2D[] m_Tragets;
        private State m_AttackState, m_MaxHealth, m_MaxEnergy;
        [SerializeField] private SliderScript healthSlider;
        [SerializeField] private SliderScript energySlider;
        [SerializeField] private Transform ProjectilePlaceHolder;
        [SerializeField] private ParicalScript Partical;
        [SerializeField] private GameObject playerVisual;
        InventoryManager inventoryManager;
        Projectile CurrentProjectile;
        private void Start()
        {

            inventoryManager = InventoryManager.Instance;
            inventoryManager.equipmentPanel.OnAddItemEvent += GetWeapon_OnAddItemEvent;
            m_AttackState = StateManager.Instance.characterState.states.Find(state => state.type == StateType.Damage);
            m_MaxHealth = StateManager.Instance.characterState.states.Find(state => state.type == StateType.MaxHealth);
            m_MaxEnergy = StateManager.Instance.characterState.states.Find(state => state.type == StateType.MaxEnergy);
            CurrentChange();
        }

        private void GetWeapon_OnAddItemEvent(EquipableItem obj)
        {
            if (obj.Properties.Type == BodyPartType.Weapon)
            {
                if (obj.EquipmentType == EquipmentType.Range)
                {

                    CurrentProjectile = ((Weapon)obj).projectile;
                }
                else
                    CurrentProjectile = null;
            }
        }

        private void Update()
        {
            if (CurrentHealth <= 0)
            {
                playerVisual.SetActive(false);
                Instantiate(Partical, playerVisual.transform.position, quaternion.identity);
                return;
            }
            if (CrossPlatformInputManager.GetButtonDown("Fire1"))
            {
                Attack();
            }
        }
        void Attack()
        {
            if (CurrentProjectile == null)
            {
                m_Tragets = Physics2D.OverlapCircleAll(AttackPoint.position, Radius, TargetLayer);
                foreach (var target in m_Tragets)
                {
                    Debug.Log("this is the target" + target.name);
                    target.GetComponent<IINteractable>().GetDamage((int)m_AttackState.amount);
                }
            }
            else
            {
                var projectile = Instantiate(CurrentProjectile, ProjectilePlaceHolder.position, quaternion.identity);
                projectile.damage += (int)StateManager.Instance.characterState.states.Find(s => s.type == StateType.Damage).amount;
                if (Mathf.Sign(projectile.velocity.x) != Mathf.Sign(transform.localScale.x))
                    projectile.velocity.x *= -1;

            }
        }


        public void RefreshSliders()
        {
            healthSlider.MaxAmount = m_MaxHealth.amount;
            energySlider.MaxAmount = m_MaxEnergy.amount;
            healthSlider.CurrentAmount = CurrentHealth;
            energySlider.CurrentAmount = CurrentEnergy;
        }

        public void CurrentChange(int energy = 0, int health = 0)
        {
            CurrentHealth += health;
            CurrentEnergy += energy;
            if (CurrentHealth > m_MaxHealth.amount)
                CurrentHealth = (int)m_MaxHealth.amount;
            if (CurrentEnergy > m_MaxEnergy.amount)
                CurrentEnergy = (int)m_MaxEnergy.amount;
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