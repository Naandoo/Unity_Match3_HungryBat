using System.Collections;
using FruitItem;
using UnityEngine;

namespace Skill
{
    public abstract class Skill : ScriptableObject
    {
        public int CurrentAmount { get; set; } // TODO: Add skills based on some system / level design
        public string Description;
        public abstract IEnumerator Execute(Fruit selectedFruit, Fruit[,] boardFruit);
    }
}
