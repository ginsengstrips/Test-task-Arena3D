using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private EventManager _eventManager;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _shotSound, _waveChangeSound, _reloadSound, _refillSound;
    private void OnEnable()
    {
        _eventManager.OnWaveChangedSound += ChangeWaveSound;
        _eventManager.OnReloadSound += ReloadSound;
        _eventManager.OnRefillHealthSound += RefillHealthSound;
    }
    private void OnDisable()
    {
        _eventManager.OnWaveChangedSound -= ChangeWaveSound;
        _eventManager.OnReloadSound -= ReloadSound;
        _eventManager.OnRefillHealthSound -= RefillHealthSound;
    }
    public void ShotSound()
    {
        _audioSource.PlayOneShot(_shotSound);
    }
    public void ChangeWaveSound()
    {
        _audioSource.PlayOneShot(_waveChangeSound);
    }
    public void ReloadSound()
    {
        _audioSource.PlayOneShot(_reloadSound);
    }
    public void RefillHealthSound()
    {
        _audioSource.PlayOneShot(_refillSound);
    }

}
