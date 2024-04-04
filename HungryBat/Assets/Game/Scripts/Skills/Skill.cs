using System.Collections;
using FruitItem;
using ScriptableVariable;
using UnityEngine;

namespace Skills
{
    public abstract class Skill : ScriptableObject
    {
        public string Name;
        public IntVariable CurrentAmount;
        public string Description;
        public abstract IEnumerator Execute(Fruit selectedFruit, Fruit[,] boardFruit);
    }
}
