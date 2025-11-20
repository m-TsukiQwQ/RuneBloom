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

    public List<ElementType> _currentEffects;

    [Header("Chill Effect")]
    [SerializeField] private float _freezeCharge = 5f;
    public float _currentChillCharge { get; private set; }
    [SerializeField] private float _chillStacksDecayTime;
    private Coroutine _chillCo;
    private Coroutine _chillDecayCo;

    [Header("Burn Effect")]
    [SerializeField] private float _burnStacksDeacayTime;
    public float _currentBurnCharge { get; private set; }
    private Coroutine _burnCo;
    [SerializeField] private float _ticksPerSecond = 2f;
    [SerializeField] private GameObject _firePrefab;
    private GameObject _fire;

    [Header("Poison Effect")]
    [SerializeField] private float _currentPoisonCharge;
    [SerializeField] private float _corrosionCharge;
    private Coroutine _poisonCo;
    private Coroutine _corrosionCo;
    [SerializeField] private GameObject _poisonPrefab;
    private GameObject _poison;

    //private Dictionary<string, StatusEffect> m_ActiveEffects = new Dictionary<string, StatusEffect>();

    public event Action<ElementType, int> OnEffectApplied;
    public event Action<ElementType> OnEffectRemoved;

    private void Awake()
    {
        _entity = GetComponent<Entity>();
        _vfx = GetComponent<EntityVFX>();
        _stats = GetComponent<EntityStats>();
        _health = GetComponent<EntityHealth>();
    }
    //poison effect

    public void ApplyPoisonEffect(float duration, float regenerationReduction, float maximumCharges, float corrosionPower)
    {
        float poisonResistance = _stats.GetElementalResistance(ElementType.Poison);
        bool isStackApplied = Random.Range(0, 100) > (poisonResistance / 2) * 100;

        if (!isStackApplied)
            return;
        _currentPoisonCharge++;

        if (_poison == null)
            _poison = Instantiate(_poisonPrefab, transform.position, Quaternion.identity, transform);

        if (_currentPoisonCharge == _corrosionCharge && _corrosionCo == null)
            _corrosionCo = StartCoroutine(CorrosionCo(duration, corrosionPower));

        _currentPoisonCharge = Mathf.Min(_currentPoisonCharge, maximumCharges);
        OnEffectApplied?.Invoke(ElementType.Poison, (int)_currentPoisonCharge);

        if ( _poisonCo != null ) 
            StopCoroutine(_poisonCo);

        _poisonCo = StartCoroutine(PoisonEffectCo(duration, regenerationReduction));

    }

    public IEnumerator CorrosionCo(float duration, float corrosionPower)
    {
        _stats.defence.armor.AddModifier(corrosionPower, "Corrosion");
        //_stats.defence.fireResistance.AddModifier(corrosionPower, "Corrosion");
        //_stats.defence.iceResistance.AddModifier(corrosionPower, "Corrosion");

        yield return new WaitForSeconds(duration);

        _stats.defence.armor.RemoveModifier("Corrosion");
        //_stats.defence.fireResistance.RemoveModifier("Corrosion");
        //_stats.defence.iceResistance.RemoveModifier( "Corrosion");

        _corrosionCo = null;

    }
    public IEnumerator PoisonEffectCo(float duration, float regenerationReduction)
    {
        Debug.Log("Poison");
        _currentEffects.Add(ElementType.Poison);
        _vfx.PlayOnStatusVfx(duration, ElementType.Poison);
        _stats.resources.healthRegenerationMultiplier.AddModifier(regenerationReduction, "Poison Stacks");

        yield return new WaitForSeconds(duration);

        _stats.resources.healthRegenerationMultiplier.RemoveModifier( "Poison Stacks");
        Debug.Log("No poison");
        StopPoisonEffect();

    }

    private void StopPoisonEffect()
    {
        _currentEffects.Remove(ElementType.Poison);
        _currentPoisonCharge = 0;
        _stats.resources.healthRegenerationMultiplier.RemoveModifier("Poison Stacks");
        OnEffectRemoved?.Invoke(ElementType.Poison);
        if (_poison != null)
        {
            Destroy(_poison);
            _poison = null;
        }
        //OnEffectRemoved?.Invoke(ElementType.Fire);

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

        _currentBurnCharge++;

        _currentBurnCharge = Mathf.Min(_currentBurnCharge, maximumCharges);
        OnEffectApplied?.Invoke(ElementType.Fire, (int)_currentBurnCharge);

        if (_burnCo != null)
            StopCoroutine(_burnCo);
        _burnCo = StartCoroutine(BurnEffectCo(duration, damage * _currentBurnCharge));


    }

    private IEnumerator BurnEffectCo(float duration, float burnDamage)
    {
        _currentEffects.Add(ElementType.Fire);
        _vfx.PlayOnStatusVfx(duration, ElementType.Fire);
        int tickCount = Mathf.RoundToInt(duration * _ticksPerSecond);

        
        float tickInterval = 1f / _ticksPerSecond;

        for (int i = 0; i < tickCount; i++)
        {
            _health.ReduceHealth(burnDamage);
            yield return new WaitForSeconds(tickInterval);
        }

        StopBurnEffect();
        _burnCo = null; 
    }
    private void StopBurnEffect()
    {
        _currentEffects.Remove(ElementType.Fire);
        _currentBurnCharge = 0;
        OnEffectRemoved?.Invoke(ElementType.Fire);
        if (_fire != null)
        {
            Destroy(_fire);
            _fire = null;
        }

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
        _currentEffects.Add (ElementType.Ice);
        _vfx.PlayOnStatusVfx(duration, ElementType.Ice);
        yield return new WaitForSeconds(duration);

        StopChillEffect();
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
        _currentEffects.Remove(ElementType.Ice);
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
        if (element == ElementType.Poison && _currentPoisonCharge >= maximumCharges)
        {
            return false;
        }

        return true;
    }
}
