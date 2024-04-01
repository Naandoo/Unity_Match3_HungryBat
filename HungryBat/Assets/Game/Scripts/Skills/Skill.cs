using System.Collections;
using FruitItem;
using UnityEngine;

namespace Skill
{
    public abstract class Skill : ScriptableObject
    {
        public string Name;
        public int CurrentAmount { get; set; } // TODO: Add skills based on level
        public string Description;
        public abstract IEnumerator Execute(Fruit selectedFruit, Fruit[,] boardFruit);
    }
}
