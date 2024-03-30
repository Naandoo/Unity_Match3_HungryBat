using UnityEngine;
using FruitItem;
using System;

namespace Level
{
    [CreateAssetMenu(fileName = "Level", menuName = "Level/Scriptables/Level")]
    public class Level : ScriptableObject
    {
        public int Number;
        public int MovementAmount;
        public Goal firstGoal, secondGoal, thirdGoal;
    }

    [Serializable]
    public class Goal
    {
        public int amount;
        public FruitID fruitID;
    }
}
