using Script.QuestSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] List<SpawnerArea> areas;
    public event Action<Quest> SetQuestEvent;
    public static SpawnerManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
    }
    public void SetQuest(Quest quest)
    {
        foreach (var area in areas)
        {
            area.SetQuestEvent.Invoke(quest);
            //area.SetQuestEvent += SetQuestEvent;
        }
    }
}
