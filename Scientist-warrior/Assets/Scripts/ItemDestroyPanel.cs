using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDestroyPanel : MonoBehaviour
{

    public event Action OnYesButtonEvent;
    public event Action OnNoButtonEvent;
    public Image ItemIcon;
    [SerializeField] private Button YesButton;
    [SerializeField] private Button NoButton;
    void Start()
    {
        YesButton.onClick.AddListener(OnYesButtonOnclick);
        NoButton.onClick.AddListener(OnNoButtonOnclick);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        OnYesButtonEvent = null;
        OnNoButtonEvent = null;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    private void OnYesButtonOnclick()
    {
        OnYesButtonEvent?.Invoke();
    }
    private void OnNoButtonOnclick()
    {
        OnNoButtonEvent?.Invoke();
    }

}
