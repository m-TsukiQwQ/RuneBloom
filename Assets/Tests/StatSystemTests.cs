using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StatSystemTests
{
    [Test]
    public void Stat_Calculates_BaseValue_Correctly()
    {
        // Arrange 

        Stat strength = new Stat();
        strength.SetBaseValue(10f);

        // Act
        float result = strength.GetValue();

        // Assert
        Assert.AreEqual(10f, result);
    }

    [Test]
    public void Stat_Adds_Modifier_Correctly()
    {
        // Arrange
        Stat strength = new Stat();
        strength.SetBaseValue(10f);

        // Act 
        strength.AddModifier(5f, "Iron Sword");
        float result = strength.GetValue();

        // Assert 
        Assert.AreEqual(15f, result);
    }

    [Test]
    public void Stat_Calculates_Complex_Formula_Correctly()
    {
        // Arrange
        Stat damage = new Stat();
        damage.SetBaseValue(100f); 

        // Act
        damage.AddModifier(50f, "Weapon"); 
        damage.AddMultiplier(10f, "Passive Skill"); 

        // Wzór: (100 + 50) * (1 + 0.10) = 150 * 1.1 = 165
        float result = damage.GetValue();

        // Assert
        Assert.AreEqual(165f, result, 0.01f); 
    }
}