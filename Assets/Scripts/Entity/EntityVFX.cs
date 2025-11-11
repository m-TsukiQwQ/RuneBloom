using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EntityVFX : MonoBehaviour
{
    private SpriteRenderer _sr;
    private Entity _entity;

    [Header("On taking damage VFX")]
    [SerializeField] private Material _onDamageMaterial;
    [SerializeField] private float _onDamageVFXDuration;
    [SerializeField] private Color _colorOnDamage1;
    [SerializeField] private Color _colorDamageText = Color.white;
    [SerializeField] private DamageText _damageTextPrefab;


    [Header("On doing damage VFX")]
    [SerializeField] private GameObject _hitVfxPrefab;

    [Header("On doing critdamage VFX")]
    [SerializeField] private GameObject _critHitVFXPrefab;

    [Header("Elements colors")]
    [SerializeField] private Color _chillVfx = Color.cyan;
    [SerializeField] private Color _burnVfx = Color.red;
    [SerializeField] private Color _poisonVfx = Color.green;
    


    private Material _originalMaterial;
    private Coroutine _onDamageVfxCoroutine;

    private void Awake()
    {
        _entity = GetComponent<Entity>();
        _sr = GetComponentInChildren<SpriteRenderer>();
        _originalMaterial = _sr.material;
    }

    public void PlayOnStatusVfx(float duration, ElementType element)
    {
        if (element == ElementType.Ice)
            StartCoroutine(PlayOnStatusVfxCo(duration, _chillVfx));
    }

    private IEnumerator PlayOnStatusVfxCo(float duration, Color color)
    {
        float tickInterval = .25f;
        float timeHasPassed = 0f;

        Color lightColor = color * 1.2f;
        Color darkColor = color * 1f;

        bool toggle = true;

        while(timeHasPassed < duration)
        {
            _sr.color = toggle ? lightColor : darkColor;
            toggle = !toggle;

            yield return new WaitForSeconds(tickInterval);
            timeHasPassed += tickInterval;
        }

        _sr.color = Color.white;
    }

    public void CreateOnHitVFX(Transform target, bool isCrit, int rotation)
    {
        GameObject vfx = Instantiate(_hitVfxPrefab, (target.position + new Vector3(0, 0.15f)), Quaternion.identity);
        if(isCrit)
        {
            GameObject critVfx = Instantiate(_critHitVFXPrefab, (target.position + new Vector3(0, 0.15f)), Quaternion.Euler(0, 0, rotation));
        }    

    }

    public void PlayOnDamageVfx()
    {
        if(_onDamageVfxCoroutine != null)
            StopCoroutine(_onDamageVfxCoroutine);

        _onDamageVfxCoroutine = StartCoroutine(OnDamageVfxCo());
    }

    private IEnumerator OnDamageVfxCo()
    {
        _onDamageMaterial.color = _colorOnDamage1;
        _sr.material = _onDamageMaterial;
        yield return new WaitForSeconds(_onDamageVFXDuration);
        _sr.material = _originalMaterial;

    }

    public void ShowDamageText(float damage, Vector2 position)
    {
        DamageText text = Instantiate(_damageTextPrefab, position, Quaternion.identity);
        text.SetDamageText(damage, _colorDamageText);

    }
    public void ShowDodgeText(Vector2 position)
    {
        DamageText text = Instantiate(_damageTextPrefab, position, Quaternion.identity);
        text.SetDodgeText(Color.white);

    }
}

