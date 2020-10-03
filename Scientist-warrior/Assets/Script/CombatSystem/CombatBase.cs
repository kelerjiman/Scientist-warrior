using UnityEngine;

namespace Script.CombatSystem
{
    public class CombatBase : MonoBehaviour, IINteractable
    {
        [Space(10)]
        [Header("Base Combat")]
        [SerializeField] internal int CurrentHealth = 1;
        [SerializeField] internal int CurrentEnergy = 1;
        [SerializeField] internal Transform AttackPoint;
        [SerializeField] internal float Radius = 1;
        [SerializeField] internal LayerMask TargetLayer;
        [SerializeField] internal float attackSpeed;
        public virtual void GetDamage(int damage)
        {
            CurrentHealth -= damage;
        }
    }
}