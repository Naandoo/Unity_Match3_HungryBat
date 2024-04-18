using System;
using System.Collections.Generic;
using UnityEngine;

namespace FruitItem
{
    [CreateAssetMenu(fileName = "FruitID", menuName = "Scriptables/Fruit", order = 1)]
    public class FruitID : ScriptableObject
    {
        [SerializeField] private Sprite _fruitSprite;
        [SerializeField] private FruitType _fruitType;

        public Sprite FruitSprite { get => _fruitSprite; private set { } }
        public FruitType FruitType { get => _fruitType; private set { } }
    }

    [Serializable]
    public class FruitColorDictionary
    {
        [SerializeField] FruitColor[] fruitColors;
        public Dictionary<FruitType, FruitColor> dictionary = new();
        public void InitializeDictionary()
        {
            dictionary.Clear();

            foreach (FruitColor fruitColor in fruitColors)
            {
                dictionary.Add(fruitColor.FruitType, fruitColor);
            }
        }
    }

    [Serializable]
    public class FruitIdDictionary
    {
        [SerializeField] FruitID[] fruitIds;
        public Dictionary<FruitType, FruitID> dictionary = new();

        public void InitializeDictionary()
        {
            dictionary.Clear();

            foreach (FruitID fruitID in fruitIds)
            {
                dictionary.Add(fruitID.FruitType, fruitID);
            }
        }
    }
}
