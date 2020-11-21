using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    private float m_CurrentAmount;
    private float m_MaxAmount;
    [SerializeField] private Image slider;
    [SerializeField] private Text CurrentText, MaxAmountText;

    public float CurrentAmount
    {
        get { return m_CurrentAmount; }
        set
        {
            m_CurrentAmount = value;
            slider.fillAmount =m_CurrentAmount / (m_MaxAmount / 100) / 100;
            CurrentText.text = m_CurrentAmount.ToString();
        }
    }

    private void Start()
    {
        CurrentAmount = m_CurrentAmount;
    }

    public float MaxAmount
    {
        get { return m_MaxAmount; }
        set
        {
            m_MaxAmount = value;
            MaxAmountText.text = Mathf.RoundToInt(m_MaxAmount).ToString();
        }
    }
}