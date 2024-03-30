using System.Collections.Generic;
using UnityEngine;
using FruitItem;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private List<Level> levels = new();
        [SerializeField] private LevelFruits levelFruits = new();
        private Dictionary<int, Level> levelDictionary = new();

        private void Awake()
        {
            InitializeDictionary();
        }

        private void InitializeDictionary()
        {
            for (int i = 0; i < levels.Count; i++)
            {
                levelDictionary.Add(i, levels[i]);
            }
        }

        private Level GetCurrentLevel(int number)
        {
            if (levelDictionary.ContainsKey(number))
            {
                return levelDictionary[number];
            }
            else
            {
                print("Doesn't contain level"); //TODO: Add a scream thanking the player for play and restart the with a button
                return null;
            }
        }

        //TODO: Design a system that starts in the game manager and call the methods in order to initiate the level.
        private void UpdateLevelScriptable(Level level)
        {
            List<FruitID> availableFruitsInLevel = new()
            {
                level.firstGoal.fruitID,
                level.secondGoal.fruitID,
                level.thirdGoal.fruitID,
            };

            levelFruits.SetAvailableFruits(availableFruitsInLevel);
        }
    }
}