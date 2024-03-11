using UnityEngine;

namespace BoardItem
{
    [CreateAssetMenu(fileName = "FruitID", menuName = "Scriptables/Fruit", order = 1)]
    public class FruitID : ScriptableObject
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private FruitType fruitType;

        public Sprite Sprite { get => _sprite; private set { } }
    }
}
