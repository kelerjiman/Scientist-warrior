using System.Collections.Generic;
using UnityEngine;

namespace Script.CharacterStatus
{
    public class StateUI : MonoBehaviour, IStateUI
    {
        [SerializeField] private List<State> statesList=new List<State>();
        [SerializeField] private RectTransform stateSlotParent;
        [SerializeField] private StateSlot stateSlot;
        public List<StateSlot> m_StateSlots;
        private void Start()
        {
            statesList = StateManager.Instance.characterState.GetCopy();
            foreach (var state in statesList)
            {
                var x=Instantiate(stateSlot, stateSlotParent);
                x.State = state;
                m_StateSlots.Add(x);
            }
        }


        public void RemoveState(State state)
        {
            foreach (var slot in m_StateSlots)
            {
                if (slot.State.type == state.type)
                {
                    slot.State.amount -= state.amount;
                    slot.State = slot.state;
                }
            }
        }

        public void AddState(State state)
        {
            foreach (var slot in m_StateSlots)
            {
                if (slot.State.type == state.type)
                {
                    slot.State.amount += state.amount;
                    slot.State = slot.state;
                }
            }
        }
    }
}