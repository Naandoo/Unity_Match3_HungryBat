using UnityEngine;
using FruitItem;
using System.Collections.Generic;
using System;
using Controllers;

namespace FruitItem
{
    public class FruitEffects : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _explosionEffectPrefab;
        [SerializeField] private ParticleSystem _essenceEffectPrefab;
        [SerializeField] private FruitColorDictionary _fruitColorDictionary;
        [SerializeField] private AudioClip _explosionFruitSFX;
        [SerializeField] private Transform _particleCollector;
        private PoolSystem<ParticleSystem> _explosionEffectPool;
        private PoolSystem<ParticleSystem> _essenceEffectPool;
        private FruitEffects() { }
        public static FruitEffects Instance { get; private set; }

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
            InitializeVariables();
        }

        private void Start()
        {
            GameEvents.Instance.OnFruitsExplodedEvent.AddListener(PlayExplosionSound);
        }

        private void OnDestroy()
        {
            GameEvents.Instance.OnFruitsExplodedEvent.RemoveListener(PlayExplosionSound);
        }

        private void InitializeVariables()
        {
            _fruitColorDictionary.InitializeDictionary();
            _explosionEffectPool = new(_explosionEffectPrefab, 64, this.transform);
            _essenceEffectPool = new(_essenceEffectPrefab, 64, this.transform);

        }

        public void PlayExplosionParticle(Vector3 position, FruitType fruitType)
        {
            ParticleSystem explosionFeedback = _explosionEffectPool.Get();
            explosionFeedback.transform.position = position;
            UpdateParticleMainColor(explosionFeedback, fruitType);
            explosionFeedback.Play();
            PlayEssenceParticle(position, fruitType);
        }

        public void PlayEssenceParticle(Vector3 position, FruitType fruitType)
        {
            ParticleSystem essenceFeedback = _essenceEffectPool.Get();
            essenceFeedback.transform.position = position;

            Color essenceColor = _fruitColorDictionary.dictionary[fruitType].EssenceColor;
            Color mainColor = _fruitColorDictionary.dictionary[fruitType].Color;

            ParticleSystem.MainModule main = essenceFeedback.main;
            main.startColor = essenceColor;

            ParticleSystem.TrailModule trail = essenceFeedback.trails;
            trail.colorOverLifetime = new ParticleSystem.MinMaxGradient(mainColor);
            trail.colorOverTrail = new ParticleSystem.MinMaxGradient(mainColor);

            ParticleSystem.TriggerModule triggerModule = essenceFeedback.trigger;
            triggerModule.AddCollider(_particleCollector);
            essenceFeedback.Play();
        }

        private void PlayExplosionSound(List<Fruit> fruits)
        {
            GameSounds.Instance.OnValidPlay(_explosionFruitSFX, enablePitchVariation: true);
        }

        private void UpdateParticleMainColor(ParticleSystem particleSystem, FruitType fruitType)
        {
            Color particleColor = _fruitColorDictionary.dictionary[fruitType].Color;

            ParticleSystem[] particleSystems = particleSystem.GetComponentsInChildren<ParticleSystem>();

            foreach (ParticleSystem particle in particleSystems)
            {
                ParticleSystem.MainModule main = particle.main;
                main.startColor = particleColor;
            }
        }
    }

    [Serializable]
    public class FruitColorDictionary
    {
        [SerializeField] FruitColor[] fruitColors;
        public Dictionary<FruitType, FruitColor> dictionary = new();
        public void InitializeDictionary()
        {
            foreach (FruitColor fruitColor in fruitColors)
            {
                dictionary.Add(fruitColor.FruitType, fruitColor);
            }
        }
    }
}

