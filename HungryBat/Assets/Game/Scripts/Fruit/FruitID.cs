using UnityEngine;
using UnityEngine.UI;

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
}
