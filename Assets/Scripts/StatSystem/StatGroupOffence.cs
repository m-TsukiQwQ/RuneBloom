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
    public StatGroupOffenceIce ice;
    public StatGroupOffenceFire fire;
    public Stat iceDamage;
    public Stat poisonDamage;


}
