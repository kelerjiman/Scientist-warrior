using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.CharacterStatus
{
    public class StateUI : MonoBehaviour, IStateUI
    {
        [SerializeField] private CharacterState characterState;
        [SerializeField] private RectTransform stateSlotParent;
        [SerializeField] private StateSlot stateSlot;
        public List<StateSlot> m_StateSlots;

        private void OnValidate()
        {
            if (stateSlotParent != null && stateSlotParent.childCount == 0)
                for (int i = 0; i < characterState.states.Count; i++)
                {
                    m_StateSlots.Add(Instantiate(stateSlot, stateSlotParent));
                    m_StateSlots[i].state = characterState.states[i];
                }
        }

        private void Awake()
        {
            for (int i = 0; i < characterState.states.Count; i++)
            {
                m_StateSlots.Add(Instantiate(stateSlot, stateSlotParent));
                m_StateSlots[i].state = characterState.states[i];
            }
        }

        public void RemoveState(State state)
        {
            if (m_StateSlots.Find(slot => slot.state.type == state.type).State != null)
            {
                State temp = m_StateSlots.Find(slot => slot.state.type == state.type).State;
                temp.amount = m_StateSlots.Find(slot => slot.state.type == state.type).State.amount - state.amount;
                m_StateSlots.Find(slot => slot.state.type == state.type).State = temp;
            }
        }

        public void AddState(State state)
        {
            if (m_StateSlots.Find(slot => slot.state.type == state.type).State != null)
            {
                State temp = m_StateSlots.Find(slot => slot.state.type == state.type).State;
                temp.name = m_StateSlots.Find(slot => slot.state.type == state.type).State.name;
                temp.amount = m_StateSlots.Find(slot => slot.state.type == state.type).State.amount + state.amount;
                m_StateSlots.Find(slot => slot.state.type == state.type).State = temp;
            }
        }
    }
}