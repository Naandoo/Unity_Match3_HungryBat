using UnityEngine;
using FruitItem;
using DG.Tweening;
using LevelData;
using System.Collections.Generic;

namespace Board
{
    public class BoardFruitPool : MonoBehaviour
    {
        [SerializeField] private Fruit _boardFruit;
        [SerializeField] private BoardSubscriber _boardListener;
        [SerializeField] private BoardGrid _boardGrid;
        private PoolSystem<Fruit> _poolSystem;
        public PoolSystem<Fruit> PoolSystem { get => _poolSystem; private set { } }
        private List<FruitID> _availableFruitIDs;


        public void Initialize(int poolSize)
        {
            _poolSystem = new PoolSystem<Fruit>(_boardFruit, poolSize, transform);
            DOTween.SetTweensCapacity(500, 500);
        }

        public void SetAvailableFruits(List<FruitID> levelFruits)
        {
            _availableFruitIDs = levelFruits;
        }
        public Fruit GetRandomFruit()
        {
            Fruit fruit = _poolSystem.Get();

            int randomFruitID = Random.Range(0, _availableFruitIDs.Count);
            fruit.SetFruitID(_availableFruitIDs[randomFruitID]);

            return fruit;
        }

        public void OnReleasedFruit(Fruit fruit)
        {
            _boardListener.UnsubscribeEventsIn(fruit);
            _poolSystem.Return(fruit);
        }
    }
}
