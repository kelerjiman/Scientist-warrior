using DG.Tweening;
using Script;
using Script.QuestSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] Npc prefab;
    [SerializeField] int id;
    [SerializeField] int level;
    [SerializeField] int maxAmount;
    [SerializeField] int currentAmount;
    [SerializeField] List<Npc> children;
    [SerializeField] float Delay;
    [SerializeField] SpriteRenderer sp;
    [SerializeField] float Raduis = 2;
    private float time;
    private bool allow = true;
    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        sp.enabled = false;
        StartChildren(maxAmount);
    }
    private void Update()
    {
        if (!allow)
            return;
        if (Time.time >= time + Delay)
        {
            time = Time.time;
            RespawnNpc();
        }
    }

    private void OnDieEvent(Npc npc)
    {
        if (npc.spawner != this)
            return;
        currentAmount = GetComponentsInChildren<Npc>().Count();
    }
    private void StartChildren(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var npc = Instantiate(prefab, transform.position, Quaternion.identity, transform);
            npc.spawner = this;
            npc.DieEvent += OnDieEvent;
            npc.IsDead = true;
            npc.gameObject.SetActive(false);
            children.Add(npc);
        }
        currentAmount = 0;

    }
    public void RespawnNpc()
    {

        if (currentAmount >= maxAmount)
            return;
        var child = children.FirstOrDefault(ch => ch.IsDead == true);
        if (child != null)
        {
            var tr = transform.position;
            child.transform.position =
                new Vector2(tr.x + Random.Range(0, Raduis), tr.y + Random.Range(0, Raduis));
            child.Revive();
            currentAmount = GetComponentsInChildren<Npc>().Count();
        }
    }
    public Npc GetNpc()
    {
        return prefab;
    }
    //public void SetQuest(Quest quest)
    public void SetQuest(int questId)
    {
        Debug.Log("SetQuest Spawner");
        foreach (var child in children)
        {
            Debug.Log("SetQuest Spawner");
            //child.quest = quest;
            child.questId = questId;
        }
    }
    public void SetNpcItem(Item item)
    {
        foreach (var child in children)
        {
            //child.quest = quest;
            var _item = child.DropItems.FindAll(itemProp => itemProp.item == item);
            if (_item.Count != 0)
                return;
            child.DropItems.Add(new ItemProp
            {
                item = item.GetCopy(),
                amount = Random.Range(1, item.MaxStack)
            });
        }
    }
    public void RemoveNpcItem(Item item)
    {
        foreach (var child in children)
        {
            var _item = child.DropItems.Find(itemProp => itemProp.item == item);
            if (_item == null)
                return;
            child.DropItems.Remove(_item);
        }
    }

}
