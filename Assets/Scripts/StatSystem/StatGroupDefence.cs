using System;
using UnityEngine;

[Serializable]
public class StatGroupDefence
{
    ///physical defense
    public Stat armor;
    public Stat evasion;

    //elemental resistance
    public Stat fireResistance;
    public Stat iceResistance;
    public Stat lightningResistance;
}
