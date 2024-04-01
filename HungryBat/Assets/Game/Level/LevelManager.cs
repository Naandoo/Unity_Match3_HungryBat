using System.Collections.Generic;
using UnityEngine;
using FruitItem;

namespace LevelData
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private List<Level> levels = new();
        [SerializeField] private LevelFruits levelFruits;
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
            int number = PlayerPrefs.GetInt("savedLevel", 1);

            if (levelDictionary.ContainsKey(number))
            {
                return levelDictionary[number];
            }
            else
            {
                //TODO: Add a scream thanking the player for play and restart the with a button
                print("Doesn't contain level");
                return null;
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
            };

            levelFruits.SetAvailableFruits(availableFruitsInLevel);
        }
    }
}