using UnityEngine;
using System.Collections.Generic;
using ScriptableVariables;

namespace Controllers
{

    public class GameSounds : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSourceWithoutPitchVariation;
        [SerializeField] private AudioSource _audioSourceWithPitchVariation;
        [SerializeField] private AudioSource _audioSourceBackgroundMusic;
        private Dictionary<AudioClip, float> _soundsTimeRegister = new();
        private Dictionary<AudioClip, float> _soundsPitchRegister = new();
        [SerializeField] private float _soundDelay;
        [SerializeField] private float _pitchDelay;
        [SerializeField] private float _pitchIncreaseAmount;
        [SerializeField] private BoolVariable _soundAvailable;
        [SerializeField] private AudioClip _defaultBackgroundMusic;
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
            _soundAvailable.Value = true;
        }

        public void OnValidPlay(AudioClip sound, bool enablePitchVariation)
        {
            AudioSource audioSource;

            if (!enablePitchVariation)
            {
                if (_soundsTimeRegister.ContainsKey(sound))
                {
                    if (WithinPlayableInterval(sound)) _soundsTimeRegister[sound] = Time.time;
                    else return;
                }
                else _soundsTimeRegister.Add(key: sound, value: Time.time);
                audioSource = _audioSourceWithoutPitchVariation;
            }
            else
            {
                UpdatePitch(sound);
                audioSource = _audioSourceWithPitchVariation;
            }

            audioSource.PlayOneShot(sound);
        }

        private bool WithinPlayableInterval(AudioClip sound) => _soundsTimeRegister[sound] + _soundDelay < Time.time;

        private void UpdatePitch(AudioClip sound)
        {
            if (_soundsPitchRegister.ContainsKey(sound))
            {
                if (WithinPitchVariationInterval(sound))
                {
                    _audioSourceWithPitchVariation.pitch += _pitchIncreaseAmount;
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
        private void NormalizePitch() => _audioSourceWithPitchVariation.pitch = 1;

        public void TriggerSoundEnable()
        {
            _soundAvailable.Value = !_soundAvailable.Value;
            TriggerAudioListeners(_soundAvailable.Value);
        }

        private void TriggerAudioListeners(bool value)
        {
            _audioSourceBackgroundMusic.mute = !value;
            _audioSourceWithoutPitchVariation.mute = !value;
            _audioSourceWithPitchVariation.mute = !value;
        }

        public void SwitchBackgroundMusic(AudioClip audioClip, bool loopEnabled)
        {
            _audioSourceBackgroundMusic.clip = audioClip;
            _audioSourceBackgroundMusic.loop = loopEnabled;
            _audioSourceBackgroundMusic.Play();
        }

        public void PlayDefaultBackgroundMusic()
        {
            SwitchBackgroundMusic(audioClip: _defaultBackgroundMusic, loopEnabled: true);
        }
    }
}
