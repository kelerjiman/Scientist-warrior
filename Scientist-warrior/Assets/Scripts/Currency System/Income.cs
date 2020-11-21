using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Income
{
    [SerializeField] int xp = 1;
    [SerializeField] int MainCoin = 1;
    public int GetCoin()
    {
        return MainCoin;
    } 
    public int GetXp()
    {
        return xp;
    }
}
