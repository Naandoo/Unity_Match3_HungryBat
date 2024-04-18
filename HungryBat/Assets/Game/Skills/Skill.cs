using System.Collections;
using FruitItem;
using ScriptableVariables;
using UnityEngine;

namespace Skills
{
    public abstract class Skill : ScriptableObject
    {
        public string Name;
        public IntVariable CurrentAmount;
        public string Description;
        public abstract IEnumerator Execute(Fruit selectedFruit, Fruit[,] boardFruit, Vector3 fruitPosition);
    }
}
