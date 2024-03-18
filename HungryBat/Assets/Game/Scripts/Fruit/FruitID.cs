using UnityEngine;

namespace FruitItem
{
    [CreateAssetMenu(fileName = "FruitID", menuName = "Scriptables/Fruit", order = 1)]
    public class FruitID : ScriptableObject
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private FruitType _fruitType;

        public Sprite Sprite { get => _sprite; private set { } }
        public FruitType FruitType { get => _fruitType; private set { } }
    }
}
