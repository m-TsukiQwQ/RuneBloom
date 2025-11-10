using UnityEngine;

public class EntityStatusHandler : MonoBehaviour
{
    private ElementType _currentEffect = ElementType.None;

    public void ApplyChillEffect(float duration, float slowMultiplier)
    {
        Debug.Log("Chill effect applied");
    }
    public bool CanBeApplied(ElementType element)
    {
        return _currentEffect == ElementType.None;
    }
}
