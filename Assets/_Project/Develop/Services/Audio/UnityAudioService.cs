using UnityEngine;
public class UnityAudioService : IAudioService
{
    private AudioSource _sfxSource;
    private AudioSource _musicSource;
    
    private float _masterVolume = 1f;
    private float _sfxMultiplier = 1f;
    private float _musicMultiplier = 0.4f;

    public UnityAudioService()
    {
        GameObject audioObj = new GameObject("GlobalAudioSystem");
        Object.DontDestroyOnLoad(audioObj);

        _sfxSource = audioObj.AddComponent<AudioSource>();
        _musicSource = audioObj.AddComponent<AudioSource>();
        _musicSource.loop = true;

        ApplyVolumes();
    }

    public void SetVolume(float volume)
    {
        _masterVolume = volume;
        ApplyVolumes();
    }

    private void ApplyVolumes()
    {
        // Музыка умножается на понижающий коэффициент
        _musicSource.volume = _masterVolume * _musicMultiplier;
        // Эффекты всегда громче музыки
        _sfxSource.volume = _masterVolume * _sfxMultiplier;
    }

    public void PlaySound(string key)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Sounds/{key}");
        if (clip != null) _sfxSource.PlayOneShot(clip, _sfxSource.volume);
    }

    public void PlayMusic(string key)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Music/{key}");
        if (clip != null)
        {
            if (_musicSource.clip == clip) return;
            _musicSource.clip = clip;
            _musicSource.Play();
        }
    }

    public void StopMusic()
    {
        if (_musicSource != null)
        {
            _musicSource.Stop();
            _musicSource.clip = null; 
        }
    }
}