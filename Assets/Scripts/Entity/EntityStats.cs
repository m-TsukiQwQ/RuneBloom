using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public Stat maxHealth;
    public Stat maxHunger;
    public StatGroupGeneral general;
    public StatGroupOffence offence;
    public StatGroupDefence defence;



    public float GetMaxHealth() => maxHealth.GetValue();
    public float GetMaxHunger() => maxHunger.GetValue();

    public float GetPhysicalDamage(out bool isCrit)
    {
        float damage = offence.damage.GetValue();

        float critChance = offence.critChance.GetValue();

        float critPower  = offence.critPower.GetValue() / 100;//total crit powerr as multiplier

        isCrit = Random.Range(1, 100) <= critChance;

        return isCrit? damage * critPower : damage;
    }
    public float GetEvasion()
    {
        float evasion = defence.evasion.GetValue();
        float evasionCap = 85;
        return Mathf.Clamp(evasion, 0, evasionCap);
    }    
}
