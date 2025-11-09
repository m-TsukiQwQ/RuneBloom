using System;
using UnityEngine;

[Serializable]
public class StatGroupResources
{
    [Header("Health")]
    public Stat maxHealth;
    public Stat healthRegeneration;

    [Space]

    [Header("Hunger")]
    public Stat maxHunger;


    [Header("MagicPower")]
    public Stat maxMagicPower;
    public Stat magicPowerRegeneration;
}
