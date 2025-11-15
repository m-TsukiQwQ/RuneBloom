using System;
using UnityEngine;

[Serializable]
public class StatGroupOffence
{
    [Header("Physical")]
    public Stat physicalDamage;
    public Stat critPower;
    public Stat critChance;
    public Stat armorReduction;
    public Stat attackSpeed;

    [Space]

    [Header("Elemental")]
    public StatGroupOffenceIce ice;
    public StatGroupOffenceFire fire;
    public StatGroupOffencePoison poison;


}

[Serializable]
public class StatGroupOffenceIce
{
    public Stat iceDamage;
    public Stat slowDownMultiplier;
    public Stat slowDownDuration;
    public Stat maxSlowDownStacks;
}

[Serializable]
public class StatGroupOffenceFire
{
    public Stat fireDamage;
    public Stat burnDuration;
    public Stat maxBurnStacks;
    public Stat burnDamage;

}

[Serializable]
public class StatGroupOffencePoison
{
    public Stat poisonDamage;
    public Stat healthRegenerationReduction;
    public Stat armorCorrosion;
    public Stat maxPoisonStack;

}
