using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace Script.CharacterStatus
{
    public class StateSlot : MonoBehaviour
    {
        public State state;

        public State State
        {
            get { return state; }
            set
            {
                state = value;
                if (state != null )
                {
                    NameText.text = state.name;
                    ValueText.text = state.amount.ToString();
                }
            }
        }

        [SerializeField] Text NameText;
        [SerializeField] Text ValueText;
        private void OnValidate()
        {
            if (state != null)
            {
                NameText.text = state.name;
                ValueText.text = state.amount.ToString();
            }
        }

        private void Start()
        {
            if (state != null)
            {
                NameText.text = state.name;
                ValueText.text = state.amount.ToString();
            }
        }
    }
}