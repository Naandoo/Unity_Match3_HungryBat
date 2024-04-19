using UnityEngine;
using FruitItem;
using DG.Tweening;
using System.Collections.Generic;

namespace Board
{
    public class BoardFruitPool : MonoBehaviour
    {
        [SerializeField] private Fruit _boardFruitObject;
        [SerializeField] private BoardSubscriber _boardListener;
        [SerializeField] private BoardGrid _boardGrid;
        private PoolSystem<Fruit> _poolSystem;
        public PoolSystem<Fruit> PoolSystem { get => _poolSystem; private set { } }
        private List<FruitID> _availableFruitIDs;


        public void Initialize(int poolSize)
        {
            _poolSystem = new PoolSystem<Fruit>(_boardFruitObject, poolSize, transform);
            DOTween.SetTweensCapacity(500, 500);
        }

        public void SetAvailableFruits(List<FruitID> levelFruits)
        {
            _availableFruitIDs = levelFruits;
        }

        public Fruit GetRandomFruit(int column, int row, bool distinctNeighbor)
        {
            Fruit fruit = _poolSystem.Get();

            if (!distinctNeighbor)
            {
                int randomFruitID = Random.Range(0, _availableFruitIDs.Count);
                fruit.SetFruitID(_availableFruitIDs[randomFruitID]);
            }
            else
            {
                Fruit leftFruit = _boardGrid.GetFruit(Mathf.Max(fruit.Column - 1, 0), row);
                Fruit belowFruit = _boardGrid.GetFruit(column, Mathf.Max(fruit.Row - 1, 0));

                List<FruitID> fruitIDsExclusive = new();
                fruitIDsExclusive.AddRange(_availableFruitIDs);

                if (leftFruit != null) fruitIDsExclusive.Remove(leftFruit.FruitID);
                if (belowFruit != null) fruitIDsExclusive.Remove(belowFruit.FruitID);
                fruitIDsExclusive.Remove(fruit.FruitID);


                int randomFruitID = Random.Range(0, fruitIDsExclusive.Count);
                fruit.SetFruitID(fruitIDsExclusive[randomFruitID]);
            }

            return fruit;
        }

        public void ReleaseFruit(Fruit fruit)
        {
            _boardListener.UnsubscribeEventsIn(fruit);
            _poolSystem.Return(fruit);
        }
    }
}
