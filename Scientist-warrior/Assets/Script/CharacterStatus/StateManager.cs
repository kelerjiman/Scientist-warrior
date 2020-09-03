using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.CharacterStatus
{
    public class StateManager : MonoBehaviour
    {
        public CharacterState characterState;
        [SerializeField] private StateUI m_StateUi;
        
        public PlayerHealth m_PlayerHealth;
        public static StateManager Instance;
        void Start()
        {
            characterState.restore();
            Instance = this;
        }

        public void AddState(List<State> obj)
        {
            
            foreach (var state in obj)
            {
                m_StateUi.AddState(state);
            }
            characterState.Add(obj);
            m_PlayerHealth.RefreshSliders();
        }

        public void RemoveState(List<State> obj)
        {
            
            foreach (var state in obj)
            {
                m_StateUi.RemoveState(state);
            }
            characterState.Remove(obj);
            m_PlayerHealth.RefreshSliders();
        }
    }
}