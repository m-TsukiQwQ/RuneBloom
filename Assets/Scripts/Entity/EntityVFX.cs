using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;

public class EntityVFX : MonoBehaviour
{
    private SpriteRenderer _sr;

    [Header("On taking damage VFX")]
    [SerializeField] private Material _onDamageMaterial;
    [SerializeField] private float _onDamageVFXDuration;
    [SerializeField] private Color _colorOnDamage1;
    [SerializeField] private Color _colorDamageText = Color.white;

    [SerializeField] private DamageText _damageTextPrefab;

    private Material _originalMaterial;
    private Coroutine _onDamageVfxCoroutine;

    private void Awake()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
        _originalMaterial = _sr.material;
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
}

