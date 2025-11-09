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
        float critPower  = offence.critPower.GetValue() / 100; //total crit powerr as multiplier

        isCrit = Random.Range(1, 100) <= critChance;

        return isCrit? damage * critPower : damage;
    }

    public float GetElementalDamage(out ElementType element)
    {
        float fireDamage = offence.fireDamage.GetValue();
        float iceDamage = offence.iceDamage.GetValue();
        float poisonDamage = offence.poisonDamage.GetValue();

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
}
