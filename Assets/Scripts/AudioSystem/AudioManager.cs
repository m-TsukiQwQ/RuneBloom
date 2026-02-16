using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioDatabaseSO _audioDb;
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioClip _sfxSource;

    public static AudioManager instance;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(string soundName, AudioSource sfxSource)
    {
        var data = _audioDb.Get(soundName);
        if (data == null) return;

        var clip = data.GetRandomClip();
        if (clip == null) return;

        sfxSource.pitch = Random.Range(data.standardPitch - .3f, data.standardPitch + .3f);
        sfxSource.clip = clip;
        sfxSource.PlayOneShot(clip);

    }
}
