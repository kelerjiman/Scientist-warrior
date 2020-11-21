using JetBrains.Annotations;
using Script.QuestSystem;
using System;
using System.Collections;
using UnityEngine;

namespace Script.CombatSystem
{
    public class EnemyCombat : CombatBase
    {
        [Space(30)]
        [Header("EnemyCombat")]
        [SerializeField]
        private Transform visual;
        [SerializeField] private Animator m_Animator;
        [SerializeField] private int moveSpeed = 1;
        [SerializeField] private Transform attackArea;
        [SerializeField] private float attackAreaRadius;
        [SerializeField] private int damage = 1;
        [SerializeField] private SliderScript healthSlider;
        [SerializeField] private SliderScript energySlider;
        [SerializeField] private GameObject Partical;
        private Vector2 m_DefPos;
        private Collider2D m_Target, m_AttackAreaTarget;
        private AnimationController m_Ac;
        private Npc Parent;
        public Collider2D Target
        {
            get { return m_Target; }
            set { m_Target = value; }
        }
        private bool m_TimeToAttack = false;


        private void Start()
        {
            Parent = transform.parent.GetComponent<Npc>();
            Parent.ReviveEvent += Revive;
            m_Ac = new AnimationController(m_Animator);
            m_DefPos = transform.position;
            MaxEnergy = CurrentEnergy;
            MaxHealth = CurrentHealth;
            if (energySlider != null)
                energySlider.MaxAmount = MaxEnergy;
            if (healthSlider != null)
                healthSlider.MaxAmount = MaxHealth;

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
                if (!m_TimeToAttack)
                    MoveToTarget(m_AttackAreaTarget.gameObject.transform.position);
            }
            else
            {
                MoveToTarget(m_DefPos);
                m_TimeToAttack = false;
            }
            Attack();
        }

        void Attack()
        {
            Target = Physics2D.OverlapCircle(AttackPoint.position, Radius, TargetLayer);
            if (Target != null)
            {
                m_TimeToAttack = true;

            }
            else
            {

                m_TimeToAttack = false;
            }
            m_Ac.PlayAttackAnimation(m_TimeToAttack);

        }
        public void NextAttack()
        {
            if (Target != null)
                Target.GetComponent<IINteractable>().GetDamage(damage);
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
            if (CurrentHealth == 0)
                Die();
        }

        public void RefreshSliders()
        {
            healthSlider.CurrentAmount = CurrentHealth;
            if (energySlider == null)
                return;
            energySlider.CurrentAmount = CurrentEnergy;
        }

        void Die()
        {
            m_Ac.PlayDieAnimation();
            Instantiate(Partical).transform.position = gameObject.transform.position;
            Parent.Die(transform);
        }
        private void Revive()
        {
            CurrentEnergy = MaxEnergy;
            CurrentHealth = MaxHealth;
            RefreshSliders();
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