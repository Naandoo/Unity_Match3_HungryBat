using FruitItem;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class GameEvents : MonoBehaviour
{
    private GameEvents() { }
    public static GameEvents Instance { get; private set; }
    public UnityEvent OnWinEvent, OnWinWithExtraMovements, OnLoseEvent;
    public UnityEvent OnFruitMovedEvent;
    public OnFruitsExploded OnFruitsExplodedEvent = new();

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
}

public class OnFruitsExploded : UnityEvent<List<Fruit>> { }