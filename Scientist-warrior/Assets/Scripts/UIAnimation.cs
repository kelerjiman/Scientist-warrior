using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class UIAnimation : MonoBehaviour
{
    public enum AnimationType
    {
        Linear,
        Circle
    }

    private Tweener tweener;
    [SerializeField] RectTransform RectTransform;
    [SerializeField] GameObject target;
    [SerializeField] Vector2 OpenPos;
    [SerializeField] Vector2 ClosePos;
    [SerializeField] bool ActiveCondition = true;
    [SerializeField] float duration;
    [SerializeField] List<Button> ButtonList;
    [SerializeField] private AnimationType Type = AnimationType.Linear;
    [SerializeField] private int CircleSpeed = 1;
    public event Action PlayAnimation;

    private void OnValidate()
    {
        ClosePos = RectTransform.anchoredPosition;
    }

    private void Start()
    {
        foreach (var button in ButtonList)
        {
            button.onClick.AddListener(ButtonOclick);
        }
    }

    private void Update()
    {
        if (Type == AnimationType.Circle)
            RectTransform.transform.Rotate(0, 0, CircleSpeed * Time.deltaTime);
    }

    public void ActivateButtons(bool condition)
    {
        if (ButtonList != null)
            ButtonList.ForEach(x => x.interactable = condition);
    }

    public void OpenPanel()
    {
        tweener = RectTransform.DOAnchorPos(OpenPos, duration);
        gameObject.SetActive(true);
        tweener.onComplete += () => { tweener.Kill(); };
    }

    public void ClosePanel()
    {
        tweener = RectTransform.DOAnchorPos(ClosePos, duration);
        tweener.onComplete += () => { tweener.Kill(); };
    }

    public void ButtonOclick()
    {
        if (RectTransform.anchoredPosition == ClosePos)
        {
            //Debug.Log("Open");
            OpenPanel();
        }
        else
        {
            //Debug.Log("Close");
            ClosePanel();
        }
    }
}