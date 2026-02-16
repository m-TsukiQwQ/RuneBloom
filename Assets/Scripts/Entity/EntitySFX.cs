using UnityEngine;

public class EntitySFX : MonoBehaviour
{
    private AudioSource _audioSource;

    [Header("SFX Names")]
    [SerializeField] private string _attackHit;

    private void Awake()
    {
        _audioSource = GetComponentInChildren<AudioSource>();
    }

    public void PlayAttackHit()
    {
        AudioManager.instance.PlaySFX(_attackHit, _audioSource);
    }
}
