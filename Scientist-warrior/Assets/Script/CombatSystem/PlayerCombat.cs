using Script.CharacterStatus;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Script.CombatSystem
{
    public class PlayerCombat : CombatBase
    {
        private Collider2D[] m_Tragets;
        private State m_AttackState, m_MaxHealth, m_MaxEnergy;
        [SerializeField] private SliderScript healthSlider;
        [SerializeField] private SliderScript energySlider;
        private void Start()
        {
            m_AttackState = StateManager.Instance.characterState.states.Find(state => state.type == StateType.Damage);
            m_MaxHealth = StateManager.Instance.characterState.states.Find(state => state.type == StateType.MaxHealth);
            m_MaxEnergy = StateManager.Instance.characterState.states.Find(state => state.type == StateType.MaxEnergy);
        }

        private void Update()
        {
            if (CrossPlatformInputManager.GetButtonDown("Fire1"))
            {
                Debug.Log("PlayerCombat---> fire 1");
                Attack();
            }
        }

        void Attack()
        {
            m_Tragets = Physics2D.OverlapCircleAll(AttackPoint.position, Radius, TargetLayer);
            foreach (var target in m_Tragets)
            {
                    Debug.Log("this is the target" + target.name);
                    target.GetComponent<IINteractable>().GetDamage((int) m_AttackState.amount);
            }
        }

        public void RefreshSliders()
        {
            healthSlider.CurrentAmount = CurrentHealth;
            healthSlider.MaxAmount = m_MaxHealth.amount;
            energySlider.CurrentAmount = CurrentEnergy;
            energySlider.MaxAmount = m_MaxEnergy.amount;
        }

        public void CurrentChange(int energy = 0, int health = 0)
        {
            CurrentHealth += health;
            CurrentEnergy += energy;
            if (CurrentHealth > m_MaxHealth.amount)
                CurrentHealth = (int) m_MaxHealth.amount;
            if (CurrentEnergy > m_MaxEnergy.amount)
                CurrentEnergy = (int) m_MaxEnergy.amount;
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