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

    public float GetEvasion()
    {
        float baseEvasion = defence.evasion.GetValue();
        float evasionCap = 85;
        return Mathf.Clamp(baseEvasion, 0, evasionCap);
    }    
}
