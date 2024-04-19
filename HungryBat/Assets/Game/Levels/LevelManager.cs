using System.Collections.Generic;
using UnityEngine;
using FruitItem;
using Board;
using ScriptableVariables;

namespace LevelData
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private List<Level> levels = new();
        [SerializeField] private BoardFruitPool _boardFruitPool;
        private Dictionary<int, Level> levelDictionary;

        private void Awake()
        {
            InitializeDictionary();
        }

        private void InitializeDictionary()
        {
            levelDictionary = new();

            for (int i = 0; i < levels.Count; i++)
            {
                levelDictionary.Add(i + 1, levels[i]);
            }
        }

        public Level GetCurrentLevel()
        {
            int levelNumber = PlayerPrefs.GetInt("savedLevel", 1);

            if (levelDictionary.ContainsKey(levelNumber))
            {
                return levelDictionary[levelNumber];
            }
            else
            {
                PlayerPrefs.SetInt("savedLevel", 1);
                return GetCurrentLevel();
            }
        }

        public void UpdateLevelScriptable(Level level)
        {
            List<FruitID> availableFruitsInLevel = new()
            {
                level.FirstGoal.FruitID,
                level.SecondGoal.FruitID,
                level.ThirdGoal.FruitID,
                level.Obstacles.FirstFruitObstacle,
                level.Obstacles.SecondFruitObstacle,
                level.Obstacles.ThirdFruitObstacle,
            };

            _boardFruitPool.SetAvailableFruits(availableFruitsInLevel);
        }
    }
}