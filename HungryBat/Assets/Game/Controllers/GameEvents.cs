using FruitItem;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Controllers
{
    public class GameEvents : MonoBehaviour
    {
        private GameEvents() { }
        public static GameEvents Instance { get; private set; }
        public UnityEvent OnWinEvent, OnWinWithExtraMovements, OnLoseEvent, OnInitiateLevel;
        public UnityEvent OnFruitMovedEvent;
        public UnityEvent OnBoardFinishMovement;
        public OnFruitsExploded OnFruitsExplodedEvent = new();
        public UnityEvent onFruitReachedBat;

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
}