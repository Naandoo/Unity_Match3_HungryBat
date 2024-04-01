using UnityEngine;
using FruitItem;
using DG.Tweening;
using LevelData;

namespace Board
{
    public class BoardFruitPool : MonoBehaviour
    {
        [SerializeField] private Fruit _boardFruit;
        [SerializeField] private BoardSubscriber _boardListener;
        [SerializeField] private LevelFruits _levelFruits;
        private PoolSystem<Fruit> _poolSystem;
        public PoolSystem<Fruit> PoolSystem { get => _poolSystem; private set { } }

        public void Initialize(int poolSize)
        {
            _poolSystem = new PoolSystem<Fruit>(_boardFruit, poolSize, transform);
            DOTween.SetTweensCapacity(200, 200);
        }

        public Fruit GetRandomFruit()
        {
            Fruit fruit = _poolSystem.Get();
            fruit.SetFruitID(_levelFruits.GetRandomFruitID());
            return fruit;
        }

        public void OnReleasedFruit(Fruit fruit)
        {
            _boardListener.UnsubscribeEventsIn(fruit);
            _poolSystem.Return(fruit);
        }
    }
}
