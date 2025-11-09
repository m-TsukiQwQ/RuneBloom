using System;
using UnityEngine;

[Serializable]
public class StatGroupDefence
{
    [Header("Physical defense")]
    public Stat armor;
    public Stat evasion;

    [Space]

    [Header("Elemental resistance")]
    public Stat fireResistance;
    public Stat iceResistance;
    public Stat poisonResistance;
}
