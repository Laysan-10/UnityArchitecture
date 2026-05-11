using UnityEngine;

public interface IAudioService
{
    void SetVolume(float volume);
    void PlaySound(string key);
    void PlayMusic(string key, bool loop = true);
    void StopMusic();
}
