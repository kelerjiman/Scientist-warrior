using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.CharacterStatus
{
    [CreateAssetMenu(menuName = "CharacterState", fileName = "Ch_State")]
    public class CharacterState : ScriptableObject
    {
        public List<State> states;

        public void Remove(List<State> states)
        {
            foreach (var state in states)
            {
                if (this.states.Find(s => s.type == state.type) != null)
                {
                    this.states.Find(s => s.type == state.type).amount -= state.amount;
                }
            }
        }

        public void Add(List<State> states)
        {
            foreach (var state in states)
            {
                if (this.states.Find(s => s.type == state.type) != null)
                {
                    this.states.Find(s => s.type == state.type).amount += state.amount;
                }
            }
        }

        public void restore()
        {
            foreach (var s in states)
            {
                s.amount = 1;
            }
        }

        public List<State> GetCopy()
        {
            List<State> Temp = new List<State>();
            foreach (var state in states)
            {
                State x = new State
                {
                    amount = state.amount,
                    isInt = state.isInt,
                    name = state.name,
                    type = state.type
                };
                Temp.Add(x);
            }

            return Temp;
        }
    }

    [Serializable]
    public class State
    {
        public string name;
        public StateType type;
        public bool isInt;
        public float amount;

        public void setState(string name, bool isInt, float amount)
        {
            this.name = name;
            this.isInt = isInt;
            this.amount = amount;
        }
    }

    public enum StateType
    {
        Damage,
        Armor,
        MaxHealth,
        MaxEnergy,
        ColdResistance,
        HeatResistance
    }
}