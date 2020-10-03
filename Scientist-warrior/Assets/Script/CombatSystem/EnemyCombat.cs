using System;
using System.Collections;
using UnityEngine;

namespace Script.CombatSystem
{
    public class EnemyCombat : CombatBase
    {
        [Space(30)] [Header("EnemyCombat")] [SerializeField]
        private Transform visual;
        [SerializeField] private Animator m_Animator;
        [SerializeField] private int moveSpeed = 1;
        [SerializeField] private Transform attackArea;
        [SerializeField] private float attackAreaRadius;
        [SerializeField] private int damage = 1;
        [SerializeField] private SliderScript healthSlider;
        [SerializeField] private SliderScript energySlider;
        [SerializeField] private GameObject Partical;
        public Collider2D Target
        {
            get { return m_Target; }
            set { m_Target = value; }
        }
        public bool AnyDetect
        {
            get { return m_AnyDetect; }
            set
            {
                m_AnyDetect = value;
                if (m_AnyDetect)
                {
                }
                else
                {
                    MoveToTarget(m_AttackAreaTarget.gameObject.transform.position);
//                    m_Ac.Move(m_AttackAreaTarget.gameObject.transform.position, moveSpeed);
                }
            }
        }
//      private Collider2D[] m_Tragets, AttackAreaTargets;
        private Vector2 m_DefPos;
        private Collider2D m_Target, m_AttackAreaTarget;
        private AnimationController m_Ac;
        private bool m_TimeToAttack, m_AnyDetect = false;


        private void Start()
        {
            m_Ac = new AnimationController(m_Animator);
            m_DefPos = transform.position;
            if (energySlider != null)
                energySlider.MaxAmount = CurrentEnergy;
            if (healthSlider != null)
                healthSlider.MaxAmount = CurrentHealth;
        }

        private void Update()
        {
            DetectTarget();
        }

        void DetectTarget()
        {
            m_AttackAreaTarget = Physics2D.OverlapCircle(attackArea.position, attackAreaRadius, TargetLayer);
            if (m_AttackAreaTarget != null)
            {
                Attack();
                m_AnyDetect = true;
                if (!m_TimeToAttack)
                    MoveToTarget(m_AttackAreaTarget.gameObject.transform.position);
                
            }
            else
            {
                m_AnyDetect = false;
                MoveToTarget(m_DefPos);
                
            }
        }

        void Attack()
        {
            m_Target = Physics2D.OverlapCircle(AttackPoint.position, Radius, TargetLayer);
            if (m_Target != null)
            {
                m_TimeToAttack = true;
            }
            else
                m_TimeToAttack = false;

            
            m_Ac.PlayAttackAnimation(m_TimeToAttack);
        }

        //Move is correct dont Touch it any More
        void MoveToTarget(Vector2 target)
        {
            var visualTransform = visual.transform;
            var localScale = visualTransform.localScale;
            if (visual.transform.position.x > target.x && Math.Abs(Mathf.Sign(localScale.x) * 1 - 1) < 0.1)
            {
                localScale = new Vector2(-localScale.x, localScale.y);
                visualTransform.localScale = localScale;
            }

            if (visual.transform.position.x < target.x && Math.Abs(Mathf.Sign(localScale.x) * 1 - 1) > 0.1)
            {
                localScale = new Vector2(-localScale.x, localScale.y);
                visualTransform.localScale = localScale;
            }

            var position = visualTransform.position;
            position = Vector2.Lerp(position, target, moveSpeed * Time.deltaTime);
            visualTransform.position = position;
            m_Ac.PlayMoveAnimation(Math.Abs(position.x - target.x) < 0.01f
                ? AnimationClipType.Idle
                : AnimationClipType.Move);
        }

        public override void GetDamage(int damage)
        {
            m_Ac.PlayHitAnimation();
            base.GetDamage(damage);
            RefreshSliders();
        }

        public void RefreshSliders()
        {
            healthSlider.CurrentAmount = CurrentHealth;
            if (CurrentHealth <= 0)
            {
                Die();
            }

            if (energySlider == null)
                return;
            energySlider.CurrentAmount = CurrentEnergy;
        }

        void Die()
        {
            m_Ac.PlayDieAnimation();
            Instantiate(Partical).transform.position=gameObject.transform.position;
            Destroy(gameObject.transform.parent.gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            if (AttackPoint != null)
            {
                Gizmos.DrawWireSphere(AttackPoint.position, Radius);
            }

            if (attackArea != null)
            {
                Gizmos.DrawWireSphere(attackArea.position, attackAreaRadius);
            }
        }
    }
}