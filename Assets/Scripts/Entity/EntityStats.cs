using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public float maxHealth = 100;
    public float maxHunger = 100;

    public float GetMaxHealth() => maxHealth;
    public float GetMaxHunger() => maxHunger;
}
