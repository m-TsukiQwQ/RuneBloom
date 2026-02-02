using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System;

public class EntityTests
{
    // --- ENTITY HEALTH LOGIC TESTS ---

    [Test]
    public void EntityHealth_ReduceHealth_DecreasesCurrentHealth()
    {
        // Purpose: Verify that ReduceHealth correctly subtracts value from _currentHealth.

        // Arrange
        var (entityGO, health, stats) = CreateEntityEnvironment();

        // --- CRITICAL FIX START ---
        // Manually inject the Entity dependency because Unity wiped the private field
        var entity = entityGO.GetComponent<Entity>();
        SetPrivateField(health, "_entity", entity);
        // --- CRITICAL FIX END ---

        // Setup Initial Health
        SetPrivateField(health, "_currentHealth", 100f);

        // Act
        health.ReduceHealth(30f);

        // Assert
        float currentHealth = (float)GetPrivateField(health, "_currentHealth");
        Assert.AreEqual(70f, currentHealth, 0.01f, "Health should be reduced by the exact damage amount.");

        Cleanup(entityGO);
    }

    [Test]
    public void EntityHealth_ReduceHealth_TriggersDeathAtZero()
    {
        // Purpose: Verify that IsDead becomes true when health drops to 0.

        // Arrange
        var (entityGO, health, stats) = CreateEntityEnvironment();

        // --- CRITICAL FIX START ---
        // Manually inject the Entity dependency because Unity wiped the private field
        var entity = entityGO.GetComponent<Entity>();
        SetPrivateField(health, "_entity", entity);
        // --- CRITICAL FIX END ---

        SetPrivateField(health, "_currentHealth", 10f);

        // Act
        health.ReduceHealth(10f); // Lethal damage

        // Assert
        bool isDead = (bool)GetPrivateField(health, "isDead");
        float currentHealth = (float)GetPrivateField(health, "_currentHealth");

        Assert.IsTrue(isDead, "Entity should be marked as dead when health reaches 0.");
        Assert.AreEqual(0f, currentHealth, "Health should not go below 0.");

        Cleanup(entityGO);
    }

    [Test]
    public void EntityHealth_IncreaseHealth_RespectsMultiplierAndCap()
    {
        // 1. Setup
        var (entityGO, health, stats) = CreateEntityEnvironment();

        // 2. Initialize Stats (Keep this! It's working, as seen by the 'sets' logs)
        stats.resources.maxHealth.SetBaseValue(100f);
        stats.resources.healthRegenerationMultiplier.SetBaseValue(100f);

        // 3. Set Health
        SetPrivateField(health, "_currentHealth", 50f);

        // --- CRITICAL FIX START ---
        // Manually inject the dependency because Unity wiped the private field
        SetPrivateField(health, "_stats", stats);
        // --- CRITICAL FIX END ---

        // 4. Act
        health.IncreaseHealth(20f);

        // 5. Assert
        float currentHealth = (float)GetPrivateField(health, "_currentHealth");
        Debug.Log($"Final Health: {currentHealth}"); // Should now be 70
        Assert.AreEqual(70f, currentHealth, 0.01f);

        Cleanup(entityGO);
    }

    // --- HELPER METHODS ---

    private (GameObject, EntityHealth, EntityStats) CreateEntityEnvironment()
    {
        GameObject go = new GameObject("Test_Entity");

        // 1. Add Stats first
        var stats = go.AddComponent<EntityStats>();

        // 2. Initialize the objects MANUALLY before any Awake() can fail
        stats.resources = new StatGroupResources();
        stats.resources.maxHealth = new Stat();
        stats.resources.healthRegenerationMultiplier = new Stat();
        stats.resources.healthRegeneration = new Stat();
        stats.resources.healthRegenerationInterval = new Stat();

        // 3. Set standard defaults so Awake() doesn't fail
        stats.resources.maxHealth.SetBaseValue(100f);
        stats.resources.healthRegenerationMultiplier.SetBaseValue(100f);

        // 4. NOW add Health. 
        // This triggers Awake(), which will now successfully find the initialized stats.
        var health = go.AddComponent<EntityHealth>();

        return (go, health, stats);
    }

    // Recursively finds and instantiates all class fields, specifically handling Stat objects and Lists.
    private void ForceInitializeStats(object obj)
    {
        if (obj == null) return;

        var type = obj.GetType();
        var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        foreach (var field in type.GetFields(flags))
        {
            // Skip Unity internals to prevent infinite loops or crashes
            if (field.FieldType.IsSubclassOf(typeof(Component)) || field.FieldType.IsSubclassOf(typeof(GameObject)))
                continue;

            // Only process classes (not primitives/enums/strings)
            if (field.FieldType.IsClass && field.FieldType != typeof(string))
            {
                object fieldValue = field.GetValue(obj);

                // If null, instantiate it
                if (fieldValue == null && !field.FieldType.IsAbstract && !field.FieldType.IsInterface)
                {
                    try
                    {
                        // StatModifier has no parameterless constructor, so we skip it or handle it specifically if needed.
                        // Stat and StatGroupResources HAVE parameterless constructors (implicit or explicit).
                        if (field.FieldType.GetConstructor(Type.EmptyTypes) != null)
                        {
                            fieldValue = Activator.CreateInstance(field.FieldType);
                            field.SetValue(obj, fieldValue);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning($"[TestHelper] Could not instantiate {field.Name} of type {field.FieldType}: {e.Message}");
                        continue;
                    }
                }

                // If value exists (or was just created), recurse into it
                if (fieldValue != null)
                {
                    // If it's a List, we don't recurse into elements, but ensure the list itself exists
                    if (typeof(IList).IsAssignableFrom(field.FieldType))
                    {
                        // Lists are usually initialized by constructor in the provided scripts, 
                        // but logic above 'Activator.CreateInstance' handles it if they were null.
                    }
                    else
                    {
                        // Recurse deeper (e.g. EntityStats -> StatGroupResources -> Stat)
                        ForceInitializeStats(fieldValue);
                    }
                }
            }
        }
    }

    private void SetStatValue(EntityStats stats, string groupName, string statName, float value)
    {
        var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        // 1. Get Group (e.g. resources)
        var groupField = typeof(EntityStats).GetField(groupName, flags);
        if (groupField == null) { Debug.LogError($"Group {groupName} not found"); return; }
        object groupObj = groupField.GetValue(stats);
        if (groupObj == null) { Debug.LogError($"Group object {groupName} is null"); return; }

        // 2. Get Stat (e.g. maxHealth)
        var statField = groupObj.GetType().GetField(statName, flags);
        if (statField == null) { Debug.LogError($"Stat {statName} not found in {groupName}"); return; }
        object statObj = statField.GetValue(groupObj);
        if (statObj == null) { Debug.LogError($"Stat object {statName} is null"); return; }

        // 3. Set Base Value via public method
        var setMethod = statObj.GetType().GetMethod("SetBaseValue");
        if (setMethod != null) setMethod.Invoke(statObj, new object[] { value });
        else Debug.LogError("SetBaseValue method not found on Stat");
    }

    private void Cleanup(GameObject go)
    {
        if (go != null)
            UnityEngine.Object.DestroyImmediate(go);
    }

    private void SetPrivateField(object target, string fieldName, object value)
    {
        // Search in base classes as well since _currentHealth is protected/private in base
        var field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy);
        if (field == null)
        {
            // Fallback: manually walk up the hierarchy if FlattenHierarchy doesn't catch private fields
            var type = target.GetType();
            while (type != null && field == null)
            {
                field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                type = type.BaseType;
            }
        }

        if (field != null) field.SetValue(target, value);
        else Debug.LogError($"Field {fieldName} not found on {target.GetType().Name}.");
    }

    private object GetPrivateField(object target, string fieldName)
    {
        var field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy);
        if (field == null)
        {
            var type = target.GetType();
            while (type != null && field == null)
            {
                field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                type = type.BaseType;
            }
        }
        return field?.GetValue(target);
    }
}

// --- MOCKS ---

public class MockEntity : Entity
{
    // Implementation of abstract/virtual methods if any are added in future.
    // Currently Entity class methods are not virtual enough to override much, so we keep this minimal.
    // Note: Do NOT override RecieveKnockback as it is not virtual in the base class.
}

public class MockEntityVFX : EntityVFX
{
}