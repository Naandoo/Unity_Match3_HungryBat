using UnityEngine;
using FruitItem;
using System.Collections.Generic;
using System;
using Controllers;

public class FruitEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosionEffectPrefab;
    [SerializeField] private FruitColorDictionary _fruitColorDictionary;
    [SerializeField] private AudioClip _explosionFruitSFX;
    private PoolSystem<ParticleSystem> _explosionEffectPool;
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
    }

    public void PlayExplosionParticle(Vector3 position, FruitType fruitType)
    {
        ParticleSystem explosionFeedback = _explosionEffectPool.Get();
        explosionFeedback.transform.position = position;
        UpdateParticleColor(explosionFeedback, fruitType);

        explosionFeedback.Play();
    }

    private void PlayExplosionSound(List<Fruit> fruits)
    {
        GameSounds.Instance.OnValidPlay(_explosionFruitSFX, enablePitchVariation: true);
    }

    private void UpdateParticleColor(ParticleSystem particleSystem, FruitType fruitType)
    {
        Color particleColor = _fruitColorDictionary.dictionary[fruitType];

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
    public Dictionary<FruitType, Color> dictionary = new();
    public void InitializeDictionary()
    {
        foreach (FruitColor fruitColor in fruitColors)
        {
            dictionary.Add(fruitColor.FruitType, fruitColor.Color);
        }
    }
}

[Serializable]
public class FruitColor
{
    public FruitType FruitType;
    public Color Color;
}

