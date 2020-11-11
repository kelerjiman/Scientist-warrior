using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;

namespace Script.QuestSystem
{
    //Todo bayad zamane destroy shodan (gharar shode disable shavad )be spawner khabar dade shavad 
    public class Npc : InWorldQuestTarget
    {
        public Spawner spawner;
        public List<ItemProp> DropItems;
        public ChestInWorld chestInWorld;
        public bool IsDead = false;
        public static event Action<int> NpcInteractableEvent;
        public event Action<Npc> DieEvent;
        public event Action ReviveEvent;
        public int questId = -1;
        public void Die(Transform visualTransform=null)
        {
            IsDead = true;
            if (questId > 0)
                NpcInteractableEvent?.Invoke(questId);
            DieEvent?.Invoke(this);
            if (visualTransform != null)
            {
                var chest = Instantiate(chestInWorld, visualTransform.position, quaternion.identity);
                foreach (var item in DropItems)
                {
                    chest.itemProps.Add(item);
                }
            }
            // bayad dar visual set shavad.
            gameObject.SetActive(!IsDead);
        }
        public void Revive()
        {

            IsDead = false;
            gameObject.SetActive(!IsDead);
            ReviveEvent?.Invoke();
        }
    }
}