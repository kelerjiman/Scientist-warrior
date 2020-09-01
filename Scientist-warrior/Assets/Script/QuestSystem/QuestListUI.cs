using System;
using System.Collections;
using System.Collections.Generic;
using Script.QuestSystem;
using UnityEngine;
using UnityEngine.UI;

public class QuestListUI : MonoBehaviour
{
    [SerializeField] private List<QuestButton> QuestButtons;
    [SerializeField] private RectTransform Content;
    [SerializeField] private QuestButton questButtonPrefab;
    public Action<Quest> OnQuestButtonEvent;

    public void addQuestButton(Quest quest)
    {
        var temp = Instantiate(questButtonPrefab, Content);
        temp.Quest = quest;
        temp.QuestButtonEvent += OnQuestButtonEvent;
        temp.GetComponentInChildren<Text>().text = quest.name;
        QuestButtons.Add(temp);
    }

    public void removeQuestButton(int QuestId)
    {
        foreach (var questButton in QuestButtons)
        {
            if (questButton.Quest.questId == QuestId)
                Destroy(questButton.gameObject);
            QuestButtons.Remove(questButton);
            break;
        }
    }

    public void UpdateButton(Quest quest)
    {
        foreach (var questButton in QuestButtons)
        {
            if (questButton.Quest.questId == quest.questId)
            {
                questButton.Quest = quest;
                break;
            }
        }
    }
}