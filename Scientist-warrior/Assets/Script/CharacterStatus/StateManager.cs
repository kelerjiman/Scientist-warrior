using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.CharacterStatus
{
    public class StateManager : MonoBehaviour
    {
        [SerializeField] public CharacterState characterState;
        [SerializeField] private StateUI m_StateUi;
        [SerializeField] private int currentHealth;
        [SerializeField] private int currentEnergy;
        public static StateManager Instance;
        void Start()
        {
            characterState.restore();
            currentEnergy = characterState.maxEnergy;
            currentHealth = characterState.maxHealth;
            Instance = this;
        }

        public void AddState(List<State> obj)
        {
            characterState.Add(obj);
            foreach (var state in obj)
            {
                m_StateUi.AddState(state);
            }
        }

        public void RemoveState(List<State> obj)
        {
            characterState.Remove(obj);
            foreach (var state in obj)
            {
                m_StateUi.RemoveState(state);
            }
            
        }

        //using this method for Usable Items like portions (water , mana and Heal)
        public void AddCurrentState(int healtAmount = 0, int energyAmount = 0)
        {
            currentHealth += healtAmount;
            if (currentHealth > characterState.maxHealth)
            {
                currentHealth = characterState.maxHealth;
            }

            currentEnergy += energyAmount;
            if (currentEnergy > characterState.maxEnergy)
            {
                currentEnergy = characterState.maxEnergy;
            }
        }
    }
}