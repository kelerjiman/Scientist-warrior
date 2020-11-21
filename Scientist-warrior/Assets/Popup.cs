using System;
using System.Collections;
using Script.CurrencySystem;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(UIAnimation))]
public class Popup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dataText;
    [SerializeField] private TextMeshProUGUI TitleText;
    [SerializeField] private float lifeTime=0.5f;
    private UIAnimation _uiAnimation;

    private void Start()
    {
        
        _uiAnimation = GetComponent<UIAnimation>();
        CurrencyManager.Instance.SendLevelEvent += CurrencyManagerOnSendLevelEvent;
    }

    void CurrencyManagerOnSendLevelEvent(string obj)
    {
        TitleText.text = "LEVEL UPDATED !";
        dataText.text = obj;
        _uiAnimation.OpenPanel();
        StartCoroutine(LifeCycle());
    }

    IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(lifeTime);
        _uiAnimation.ClosePanel();
    }
}