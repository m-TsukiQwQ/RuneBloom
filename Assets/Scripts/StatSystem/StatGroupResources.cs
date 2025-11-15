using System;
using UnityEngine;

[Serializable]
public class StatGroupResources
{
    [Header("Health")]
    public Stat maxHealth;
    public Stat healthRegeneration;
    public Stat healthRegenerationMultiplier;
    public Stat healthRegenerationInterval;

    [Space]

    [Header("Hunger")]
    public Stat maxHunger;


    [Header("MagicPower")]
    public Stat maxMagicPower;
    public Stat magicPowerRegeneration;
}
