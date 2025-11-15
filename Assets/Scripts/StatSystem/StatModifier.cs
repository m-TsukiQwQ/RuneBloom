using System;
using UnityEngine;

[Serializable]
public class StatModifier 
{
    public float value;
    public string source;

    public StatModifier(float value, string source)
    {
        this.value = value;
        this.source = source;
    }
}
