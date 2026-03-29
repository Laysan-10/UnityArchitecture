using UnityEngine;

public class UnityAudioService : IAudioService
{
    public void SetVolume(float volume) => Debug.Log($"Громкость установлена на: {volume}");
    public void PlaySound(string key) => Debug.Log($"Играет звук: {key}");
}