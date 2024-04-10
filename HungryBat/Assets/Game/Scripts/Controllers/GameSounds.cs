using UnityEngine;
using System.Collections.Generic;

public class GameSounds : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    private Dictionary<AudioClip, float> _recordedSounds = new();
    private float _soundDelay = 0.1f;
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

    public void OnValidPlay(AudioClip sound)
    {
        if (_recordedSounds.ContainsKey(sound))
        {
            if (WithinValidInterval(sound)) _recordedSounds[sound] = Time.time;
            else return;
        }
        else _recordedSounds.Add(key: sound, value: Time.time);

        _audioSource.PlayOneShot(sound);
    }

    private bool WithinValidInterval(AudioClip sound) => _recordedSounds[sound] + _soundDelay < Time.time;
}
