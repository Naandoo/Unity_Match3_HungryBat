using System.Collections.Generic;
using FruitItem;
using UnityEngine;

namespace LevelData
{
    [CreateAssetMenu(fileName = "LevelFruits", menuName = "Level/LevelFruits", order = 1)]
    public class LevelFruits : ScriptableObject
    {
        private List<FruitID> _availableFruits;

        public void SetAvailableFruits(List<FruitID> AvailableFruits)
        {
            _availableFruits = AvailableFruits;
        }

        public FruitID GetRandomFruitID()
        {
            int randomFruit = Random.Range(0, _availableFruits.Count);
            return _availableFruits[randomFruit];
        }
    }
}
