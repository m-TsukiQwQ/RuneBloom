using System;
using UnityEngine;

[Serializable]
public class StatGroupResources
{
    [Header("Health")]
    public Stat maxHealth = new Stat();
    public Stat healthRegeneration = new Stat();
    public Stat healthRegenerationMultiplier = new Stat();
    public Stat healthRegenerationInterval = new Stat();

    [Header("Hunger")]
    public Stat maxHunger = new Stat();

    [Header("MagicPower")]
    public Stat maxMagicPower = new Stat();
    public Stat magicPowerRegeneration = new Stat();
}