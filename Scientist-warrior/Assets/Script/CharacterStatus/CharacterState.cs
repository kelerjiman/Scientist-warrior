using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.CharacterStatus
{
    [CreateAssetMenu(menuName = "CharacterState", fileName = "Ch_State")]
    public class CharacterState : ScriptableObject
    {
        public List<State> states;
        public int maxHealth = 1;

        public int maxEnergy = 1;

        public void Remove(List<State> states)
        {
            foreach (var state in states)
            {
                this.states.Find(s => s.type == state.type).amount -= state.amount;
            }
        }

        public void Add(List<State> states)
        {
            foreach (var state in states)
            {
                this.states.Find(s => s.type == state.type).amount += state.amount;
            }
        }

        public void restore()
        {
            foreach (var s in states)
            {
                s.amount = 1;
            }
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