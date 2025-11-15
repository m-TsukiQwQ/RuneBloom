using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public StatGroupResources resources;
    public StatGroupGeneral general;
    public StatGroupOffence offence;
    public StatGroupDefence defence;


    public float GetPhysicalDamage(out bool isCrit)
    {
        float damage = offence.physicalDamage.GetValue();
        float critChance = offence.critChance.GetValue();
        float critPower = offence.critPower.GetValue() / 100; //total crit powerr as multiplier

        isCrit = Random.Range(1, 100) <= critChance;

        return isCrit ? damage * critPower : damage;
    }

    public float GetArmor(float armorReduction)
    {
        float baseArmor = defence.armor.GetValue();

        float reductionMultiplier = Mathf.Clamp(1 - armorReduction, 0, 1);
        float finalArmor = (baseArmor * reductionMultiplier) / (baseArmor + 100);
        float armorCap = 0.95f;

        return Mathf.Clamp(finalArmor, 0, armorCap);
    }

    public float GetArmorReduction()
    {
        float finalReduction = offence.armorReduction.GetValue() / 100;
        return finalReduction;
    }

    public float GetElementalDamage(out ElementType element)
    {
        float fireDamage = offence.fire.fireDamage.GetValue();
        float iceDamage = offence.ice.iceDamage.GetValue();
        float poisonDamage = offence.poison.poisonDamage.GetValue();

        float highestDamage = fireDamage;
        element = ElementType.Fire;

        if (iceDamage > highestDamage)
        {
            highestDamage = iceDamage;
            element = ElementType.Ice;
        }

        if (poisonDamage > highestDamage)
        {
            highestDamage = poisonDamage;
            element = ElementType.Poison;
        }

        if (highestDamage <= 0)
        {
            element = ElementType.None;
            return 0;
        }

        return highestDamage;
    }

    public float GetElementalResistance(ElementType element)
    {

        float baseResistance = 0;
        switch (element)
        {
            case ElementType.Ice:
                baseResistance = defence.iceResistance.GetValue();
                break;

            case ElementType.Poison:
                baseResistance = defence.poisonResistance.GetValue();
                break;

            case ElementType.Fire:
                baseResistance = defence.fireResistance.GetValue();
                break;
        }

        return baseResistance / 100;
    }


    public float GetEvasion()
    {
        float evasion = defence.evasion.GetValue();
        float evasionCap = 85;
        return Mathf.Clamp(evasion, 0, evasionCap);
    }


    public float GetMaxHealth() => resources.maxHealth.GetValue();
    public float GetMaxHunger() => resources.maxHunger.GetValue();

    public Stat GetStatByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth: return resources.maxHealth;
            case StatType.HealthRegeneration: return resources.healthRegeneration;

            case StatType.MaxHunger: return resources.maxHunger;

            case StatType.MaxMagicPower: return resources.maxMagicPower;
            case StatType.MagicPowerRegeneration: return resources.magicPowerRegeneration;

            case StatType.PhysicalDamage: return offence.physicalDamage;
            case StatType.CritPower: return offence.critPower;
            case StatType.CritChance: return offence.critChance;
            case StatType.ArmorReduction: return offence.armorReduction;
            case StatType.AttackSpeed: return offence.attackSpeed;

            case StatType.IceDamage: return offence.ice.iceDamage;
            case StatType.SlowDownMultiplier: return offence.ice.slowDownMultiplier;
            case StatType.SlowDownDuration: return offence.ice.slowDownDuration;
            case StatType.MaxSlowDownStacks: return offence.ice.maxSlowDownStacks;

            case StatType.FireDamage: return offence.fire.fireDamage;
            case StatType.BurnDuration: return offence.fire.burnDuration;
            case StatType.MaxBurnStacks: return offence.fire.maxBurnStacks;
            case StatType.BurnDamage: return offence.fire.burnDamage;

            case StatType.PoisonDamage: return offence.poison.poisonDamage;
            case StatType.HealthRegenerationReduction: return offence.poison.healthRegenerationReduction;
            case StatType.ArmorCorrosion: return offence.poison.armorCorrosion;
            case StatType.MaxPoisonStacks: return offence.poison.maxPoisonStack;

            case StatType.Armor: return defence.armor;
            case StatType.Evasion: return defence.evasion;

            case StatType.FireResistance: return defence.fireResistance;
            case StatType.IceResistance: return defence.iceResistance;
            case StatType.PoisonResistance: return defence.poisonResistance;

            default:
                Debug.Log($"StatType {type} not implemented yet.");
                return null;
        }

    }
}
