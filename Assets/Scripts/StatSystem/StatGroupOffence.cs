using System;
using UnityEngine;

[Serializable]
public class StatGroupOffence
{
    [Header("Physical")]
    public Stat physicalDamage;
    public Stat critPower;
    public Stat critChance;

    [Space]

    [Header("Elemental")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat poisonDamage;
}
