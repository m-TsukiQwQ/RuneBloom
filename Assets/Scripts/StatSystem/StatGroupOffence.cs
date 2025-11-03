using System;
using UnityEngine;

[Serializable]
public class StatGroupOffence
{
    //physical
    public Stat damage;
    public Stat critPower;
    public Stat critChance;

    //elemental
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;
}
