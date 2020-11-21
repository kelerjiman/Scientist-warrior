using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Script.CurrencySystem;

namespace Script.QuestSystem
{
    public class Npc : InWorldQuestTarget
    {
        public Spawner spawner;
        public List<ItemProp> DropItems;
        public ChestInWorld chestInWorld;
        public bool IsDead = false;
        public int questId = -1;
        [Space(5f)]
        [Header("Income Prop")]
        [SerializeField] private Income Income;
        public static event Action<int> NpcInteractableEvent;
        public event Action<Npc> DieEvent;
        public event Action ReviveEvent;
        public static event Action<Income> IncomeEvent;
        private void Start()
        {

        }
        public void Die(Transform visualTransform = null)
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
                    chest.itemProps.Add(new ItemProp
                    {
                        item = item.item.GetCopy(),
                        amount=item.amount
                    });
                }
                chest.PopUp(visualTransform.position);
            }
            IncomeEvent?.Invoke(Income);
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