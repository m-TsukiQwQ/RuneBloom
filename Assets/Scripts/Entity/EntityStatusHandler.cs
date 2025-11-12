using System;
using System.Collections;
using UnityEngine;

public class EntityStatusHandler : MonoBehaviour
{
    private Entity _entity;
    private EntityVFX _entityVfx;
    private EntityStats _stats;
    private ElementType _currentEffect = ElementType.None;

    //chill effect extra
    [SerializeField] private float _freezeCharge = 5f;
    public float _currentChillCharge { get; private set; }
    [SerializeField] private float _chillStacksDecayTime;
    private Coroutine _chillCo;
    private Coroutine _chillDecayCo;

    public event Action<ElementType, int> OnEffectApplied;
    public event Action<ElementType> OnEffectRemoved;

    private void Awake()
    {
        _entity = GetComponent<Entity>();
        _entityVfx = GetComponent<EntityVFX>();
        _stats = GetComponent<EntityStats>();
    }

    private void Update()
    {
        
    }

    public void ApplyChillEffect(float duration, float slowMultiplier, float maximumCharges)
    {
        if (_chillDecayCo != null)
            StopCoroutine(_chillDecayCo);

        _chillStacksDecayTime = duration;
        float chillResistance = _stats.GetElementalResistance(ElementType.Ice);

        _currentChillCharge++;
        OnEffectApplied?.Invoke(ElementType.Ice, (int)_currentChillCharge);
        bool didFreeze = false; 

        if (_currentChillCharge >= _freezeCharge)
        {
            _entity.Freezed();
            _currentChillCharge = 0;
            StopChillEffect();
            didFreeze = true; 
        }

        if(_chillCo != null)
            StopCoroutine(_chillCo);

        _entity.SlowDownEntity(duration,slowMultiplier);

        _chillCo = StartCoroutine(ChillEffectCo(duration));

        if (!didFreeze && _currentChillCharge > 0)
        {
            _chillDecayCo = StartCoroutine(ChillStackDecayCo());
        }
    }

    private IEnumerator ChillEffectCo(float duration)
    {
        _currentEffect = ElementType.Ice;
        _entityVfx.PlayOnStatusVfx(duration, ElementType.Ice);
        yield return new WaitForSeconds(duration);

        _currentEffect = ElementType.None;
    }
    private IEnumerator ChillStackDecayCo()
    {
        yield return new WaitForSeconds(_chillStacksDecayTime);

        Debug.Log("Chill stacks decayed.");
        StopChillEffect();
        _chillDecayCo = null;
    }


    private void StopChillEffect()
    {
        _currentEffect = ElementType.None;
        _currentChillCharge = 0;
        OnEffectRemoved?.Invoke(ElementType.Ice);

        // If we're stopping the effect, we must also stop the decay timer.
        if (_chillDecayCo != null)
        {
            StopCoroutine(_chillDecayCo);
            _chillDecayCo = null;
        }

    }

    public bool CanBeApplied(ElementType element, float maximumChargees)
    {
        if(element == ElementType.Ice && _currentChillCharge >= maximumChargees)
        {
            return false;
        }

        return true;
    }
}
