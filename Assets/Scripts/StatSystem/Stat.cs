using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private float _baseValue;
    [SerializeField] private List<StatModifier> _modifiers = new List<StatModifier>();
    [SerializeField] private List<StatModifier> _multipliers = new List<StatModifier>();

    private float _finalValue;
    private bool _needToBeRecalculated = true;
    private float _multipliersSum;

    public List<StatModifier> Modifiers() => _modifiers;
    public List<StatModifier> Multipliers() => _multipliers;

    public float GetValue()
    {
        if (_needToBeRecalculated)
        {
            _finalValue = GetFinalValue();
            _needToBeRecalculated = false;
        }

        return _finalValue;
    }


    public void AddModifier(float value, string source)
    {
        StatModifier modToAdd = new StatModifier(value, source);
        _modifiers.Add(modToAdd);
        _needToBeRecalculated = true;
    }

    public void AddMultiplier(float value, string source)
    {
        StatModifier modToAdd = new StatModifier(value, source);
        _multipliers.Add(modToAdd);
        _needToBeRecalculated = true;
    }

    public void RemoveModifier(string source)
    {
        _modifiers.RemoveAll(modifier => modifier.source == source);
        _needToBeRecalculated = true;
    }
    public void RemoveMultiplier(string source)
    {
        _multipliers.RemoveAll(multiplier => multiplier.source == source);
        _needToBeRecalculated = true;
    }

    public float GetFinalValue()
    {
        float finalValue = _baseValue;

        foreach (var modifier in _modifiers)
        {
            finalValue += modifier.value;
        }
        _multipliersSum = 100;
        foreach (var multiplier in _multipliers)
        {
            _multipliersSum += multiplier.value;
        }
        finalValue *= (_multipliersSum / 100);

        return finalValue;
    }
}

