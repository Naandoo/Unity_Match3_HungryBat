using UnityEngine;
using System.Collections.Generic;
using Controllers;
using System.Collections;
using FruitItem;
using DG.Tweening;

namespace Effects
{
    public class FruitEffects : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _explosionEffectPrefab;
        [SerializeField] private ParticleSystem _essenceEffectPrefab;
        [SerializeField] private FruitColorDictionary _fruitColorDictionary;
        [SerializeField] private AudioClip _explosionFruitSFX;
        [SerializeField] private Transform _particleCollector;
        [SerializeField] private Transform _batTransform;
        [SerializeField] private AudioClip _swapFruit;
        [SerializeField] private AudioClip _undoSwapFruit;
        private PoolSystem<ParticleSystem> _explosionEffectPool;
        private PoolSystem<ParticleSystem> _essenceEffectPool;
        private FruitEffects() { }
        public static FruitEffects Instance { get; private set; }
        private WaitForSeconds _secondsToTriggerExternalForces;

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

            _secondsToTriggerExternalForces = new(0.5f);
        }

        private void Start()
        {
            GameEvents.Instance.OnFruitsExplodedEvent.AddListener(PlayExplosionSound);
            GameEvents.Instance.onFruitReachedBat.AddListener(BatAnimationOnFruitEaten);
        }

        private void OnDestroy()
        {
            GameEvents.Instance.OnFruitsExplodedEvent.RemoveListener(PlayExplosionSound);
            GameEvents.Instance.onFruitReachedBat.RemoveListener(BatAnimationOnFruitEaten);
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
            essenceFeedback.gameObject.SetActive(true);
            essenceFeedback.transform.position = position;

            Color essenceColor = _fruitColorDictionary.dictionary[fruitType].EssenceColor;
            Color mainColor = _fruitColorDictionary.dictionary[fruitType].Color;

            ParticleSystem[] particleSystems = essenceFeedback.GetComponentsInChildren<ParticleSystem>();

            foreach (ParticleSystem particle in particleSystems)
            {
                ParticleSystem.MainModule main = particle.main;
                main.startColor = essenceColor;
            }

            ParticleSystem.TrailModule trail = essenceFeedback.trails;
            trail.colorOverLifetime = new ParticleSystem.MinMaxGradient(mainColor);
            trail.colorOverTrail = new ParticleSystem.MinMaxGradient(mainColor);

            ParticleSystem.TriggerModule triggerModule = essenceFeedback.trigger;
            triggerModule.AddCollider(_particleCollector);
            essenceFeedback.Play();
            StartCoroutine(TriggerExternalForces(essenceFeedback));
        }

        private IEnumerator TriggerExternalForces(ParticleSystem particle)
        {
            yield return _secondsToTriggerExternalForces;
            ParticleSystem.ExternalForcesModule externalForces = particle.externalForces;
            externalForces.multiplier = 1;
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

        private void BatAnimationOnFruitEaten()
        {
            _batTransform.DOShakePosition(0.5f, strength: 3);
        }

        public void PlaySwapSound() => GameSounds.Instance.OnValidPlay(_swapFruit, enablePitchVariation: false);
        public void PlayUndoSwapSound() => GameSounds.Instance.OnValidPlay(_undoSwapFruit, enablePitchVariation: false);
    }
}

