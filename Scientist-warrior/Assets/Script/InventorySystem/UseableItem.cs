using Script;
using Script.CharacterStatus;
using UnityEngine;

public enum UseableType
{
    None,
    Food,
    Drink
}

[CreateAssetMenu(fileName = "Useable Item", menuName = "Useable Item")]
public class UseableItem : Item
{
    [Space]
    [Header("useable Setting")]
    public UseableType Type;
    public int useableAmount = 1;
    public int Energy = 1;
    public int Health = 1;

    public override bool UseItem()
    {
        StateManager.Instance.m_PlayerHealth.CurrentChange(Energy,Health);
        return true;
    }

    public override void Destroy()
    {
        Destroy(this);
    }
}