using UnityEngine;
using FruitItem;
using System;

namespace LevelData
{
    [CreateAssetMenu(fileName = "Level", menuName = "Level/Scriptables/Level")]
    public class Level : ScriptableObject
    {
        public int Number;
        public int Moves;
        public Goal FirstGoal, SecondGoal, ThirdGoal;
        public Obstacles Obstacles;
        public SkillsAvailability SkillsAvailability;
    }

    [Serializable]
    public struct Goal
    {
        public int ID;
        public int Amount;
        public FruitID FruitID;
    }

    [Serializable]
    public struct Obstacles
    {
        public FruitID FirstFruitObstacle, SecondFruitObstacle, ThirdFruitObstacle;
    }

    [Serializable]
    public struct SkillsAvailability
    {
        public int BombsAmount, PotionAmount, LightningAmount;
    }
}
