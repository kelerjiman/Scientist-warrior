using System;
using System.Collections;
using System.Collections.Generic;
using Script.CharacterStatus;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerHealth : MonoBehaviour, Health
{
    [SerializeField] private int currentHealth, currentEnergy;
    [SerializeField] private GameObject partical;
    [SerializeField] private SliderScript healthBar, EnergyBar;
    [SerializeField] private CharacterState characterState;
    public static PlayerHealth Instance;
    private State m_MaxHealth, m_MaxEnergy;

    public int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = value;
            healthBar.CurrentAmount = currentHealth;
            if (currentHealth <= 0)
            {
                Instantiate(partical, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    public int CurrentEnergy
    {
        get { return currentEnergy; }
        set
        {
            currentEnergy = value;
            EnergyBar.CurrentAmount = currentEnergy;
        }
    }

    private void Start()
    {
        foreach (var state in characterState.states)
        {
            if (state.type == StateType.MaxHealth)
                m_MaxHealth = state;
            else if (state.type == StateType.MaxEnergy)
                m_MaxEnergy = state;
        }

        CurrentEnergy = (int) m_MaxEnergy.amount;
        CurrentHealth = (int) m_MaxHealth.amount;
        RefreshSliders();
        Instance = this;
    }

    public void GetDamage(int i)
    {
        Debug.Log("Get Damage " + i);
        CurrentHealth -= i;
    }

    public void ChangeCurrentHealth(int amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > m_MaxHealth.amount)
        {
            CurrentHealth = (int) m_MaxHealth.amount;
        }
    }

    public void ChangeCurrentEnergy(int amount)
    {
        CurrentEnergy += amount;
        if (CurrentEnergy > m_MaxEnergy.amount)
        {
            CurrentEnergy = (int) m_MaxEnergy.amount;
        }
    }

    public void RefreshSliders()
    {
        EnergyBar.MaxAmount = (int) m_MaxEnergy.amount;
        healthBar.MaxAmount = (int) m_MaxHealth.amount;
        healthBar.CurrentAmount = healthBar.CurrentAmount;
        EnergyBar.CurrentAmount = EnergyBar.CurrentAmount;
    }

    public void CurrentChange(int Energy, int Health)
    {
        CurrentHealth += Health;
        currentEnergy += Energy;
        if (CurrentHealth > m_MaxHealth.amount)
            CurrentHealth = (int) m_MaxHealth.amount;
        if (CurrentEnergy > m_MaxEnergy.amount)
            currentEnergy = (int) m_MaxEnergy.amount;
    }
}