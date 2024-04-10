using UnityEngine;
using System.Collections.Generic;

public class GameSounds : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    private Dictionary<AudioClip, float> _soundsTimeRegister = new();
    private Dictionary<AudioClip, float> _soundsPitchRegister = new();
    [SerializeField] private float _soundDelay;
    [SerializeField] private float _pitchDelay;
    [SerializeField] private float _pitchIncreaseAmount;
    private GameSounds() { }
    public static GameSounds Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void OnValidPlay(AudioClip sound, bool enablePitchVariation)
    {
        if (!enablePitchVariation)
        {
            if (_soundsTimeRegister.ContainsKey(sound))
            {
                if (WithinPlayableInterval(sound)) _soundsTimeRegister[sound] = Time.time;
                else return;
            }
            else _soundsTimeRegister.Add(key: sound, value: Time.time);
        }
        else
        {
            UpdatePitch(sound);
        }

        _audioSource.PlayOneShot(sound);
    }

    private bool WithinPlayableInterval(AudioClip sound) => _soundsTimeRegister[sound] + _soundDelay < Time.time;

    private void UpdatePitch(AudioClip sound)
    {
        if (_soundsPitchRegister.ContainsKey(sound))
        {
            if (WithinPitchVariationInterval(sound))
            {
                _audioSource.pitch += _pitchIncreaseAmount;
            }
            else NormalizePitch();
            _soundsPitchRegister[sound] = Time.time;
        }
        else
        {
            _soundsPitchRegister.Add(key: sound, value: Time.time);
            NormalizePitch();
        }
    }

    private bool WithinPitchVariationInterval(AudioClip sound) => Time.time - _soundsPitchRegister[sound] <= _pitchDelay;
    private void NormalizePitch() => _audioSource.pitch = 1;
}
