using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntityStatusHandler : MonoBehaviour
{
    private Entity _entity;
    private EntityVFX _vfx;
    private EntityStats _stats;
    private EntityHealth _health;

    public ElementType _currentEffect = ElementType.None;

    [Header("Chill Effect")]
    [SerializeField] private float _freezeCharge = 5f;
    public float _currentChillCharge { get; private set; }
    [SerializeField] private float _chillStacksDecayTime;
    private Coroutine _chillCo;
    private Coroutine _chillDecayCo;

    [Header("Burn Effect")]
    public float _currentBurnCharge { get; private set; }
    [SerializeField] private float _burnStacksDeacayTime;
    private Coroutine _burnCo;
    private Coroutine _burnDecayCo;
    [SerializeField] private float _ticksPerSecond = 2f;
    [SerializeField] private GameObject _firePrefab;
    private float _fireTimer;
    private GameObject _fire;

    //private Dictionary<string, StatusEffect> m_ActiveEffects = new Dictionary<string, StatusEffect>();

    public event Action<ElementType, int> OnEffectApplied;
    public event Action<ElementType> OnEffectRemoved;

    private void Awake()
    {
        _entity = GetComponent<Entity>();
        _vfx = GetComponent<EntityVFX>();
        _stats = GetComponent<EntityStats>();
        _health = GetComponent<EntityHealth>();
        _fireTimer -= Time.deltaTime;
    }


    //burn effect
    public void ApplyBurnEffect(float duration, float damage, float maximumCharges)
    {
        float fireResistance = _stats.GetElementalResistance(ElementType.Fire);
        bool isStackApplied = Random.Range(0, 100) > (fireResistance / 2) * 100;

        if (!isStackApplied)
            return;
        

        if(_fire == null)
            _fire = Instantiate(_firePrefab, transform.position, Quaternion.identity, transform);

        _fireTimer = duration;
        _currentBurnCharge++;

        _currentBurnCharge = Mathf.Min(_currentBurnCharge, maximumCharges);
        OnEffectApplied?.Invoke(ElementType.Fire, (int)_currentBurnCharge);

        if (_burnCo != null)
            StopCoroutine(_burnCo);
        _burnCo = StartCoroutine(BurnEffectCo(duration, damage * _currentBurnCharge));


    }

    private IEnumerator BurnEffectCo(float duration, float burnDamage)
    {
        _currentEffect = ElementType.Fire;
        _vfx.PlayOnStatusVfx(duration, ElementType.Fire);
        int tickCount = Mathf.RoundToInt(duration * _ticksPerSecond);

        
        float tickInterval = 1f / _ticksPerSecond;

        for (int i = 0; i < tickCount; i++)
        {
            _health.ReduceHealth(burnDamage);
            yield return new WaitForSeconds(tickInterval);
        }


        if (_fire != null)
        {
            Destroy(_fire);
            _fire = null;
        }


        StopBurnEffect();
        _burnCo = null; 
    }
    private void StopBurnEffect()
    {
        _currentEffect = ElementType.None;
        _currentBurnCharge = 0;
        OnEffectRemoved?.Invoke(ElementType.Fire);

    }

    //chill effect
    public void ApplyChillEffect(float duration, float slowMultiplier, float maximumCharges)
    {
        float chillResistance = _stats.GetElementalResistance(ElementType.Ice);
        bool isStackApplied = Random.Range(0, 100) > (chillResistance / 2) * 100;

        if (!isStackApplied) 
            return;
        float finalSlowMultiplier = slowMultiplier * (1 - chillResistance);

        if (_chillDecayCo != null)
            StopCoroutine(_chillDecayCo);

        _chillStacksDecayTime = duration;


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

        _entity.SlowDownEntity(duration, finalSlowMultiplier);

        _chillCo = StartCoroutine(ChillEffectCo(duration));

        if (!didFreeze && _currentChillCharge > 0)
        {
            _chillDecayCo = StartCoroutine(ChillStackDecayCo());
        }
    }

    private IEnumerator ChillEffectCo(float duration)
    {
        _currentEffect = ElementType.Ice;
        _vfx.PlayOnStatusVfx(duration, ElementType.Ice);
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

    public bool CanBeApplied(ElementType element, float maximumCharges)
    {
        if(element == ElementType.Ice && _currentChillCharge >= maximumCharges)
        {
            return false;
        }

        if (element == ElementType.Fire && _currentBurnCharge >= maximumCharges)
        {
            return false;
        }

        return true;
    }
}
