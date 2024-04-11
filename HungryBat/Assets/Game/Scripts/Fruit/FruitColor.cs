using UnityEngine;

namespace FruitItem
{
    [CreateAssetMenu(fileName = "FruitColor", menuName = "Scriptables/Fruit/FruitColor")]
    public class FruitColor : ScriptableObject
    {
        public FruitType FruitType;
        public Color Color;
        public Color EssenceColor;
    }
}