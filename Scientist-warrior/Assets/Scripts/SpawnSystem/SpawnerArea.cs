using Script.QuestSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerArea : MonoBehaviour
{
    [SerializeField] List<Spawner> spawners;
    [SerializeField] int id;
    public Action<Quest> SetQuestEvent;
    private void Start()
    {
        SetQuestEvent += SpawnerArea_SetQuestEvent;
    }

    private void SpawnerArea_SetQuestEvent(Quest quest)
    {
        if (quest == null || quest.Npc == null)
            return;
        //SetQuest(((Npc)quest.InWorldQuestTarget), quest);
        SetQuest(quest.Npc.GetQuestTarget().TargetId, quest);
    }

    //public bool SetQuest(Npc npc,Quest quest)
    public void SetQuest(int targetId, Quest quest)
    {
        //Dorost kar mikone dast nazan
        foreach (var sp in spawners)
        {
            if (sp.GetNpc().GetQuestTarget().TargetId == targetId)
            {
                if (sp != null)
                {
                    //sp.SetQuest(quest);
                    if (quest.type == QuestType.Killing)
                    {
                        sp.SetQuest(quest.questId);
                    }
                    else
                    {
                        if (quest.type == QuestType.Gathering && quest.Amount < quest.MaxAmount)
                            sp.SetNpcItem(quest.QuestItem);
                        else
                            sp.RemoveNpcItem(quest.QuestItem);
                    }
                }
            }
        }
    }
}
