using System;
using System.Collections.Generic;
using FruitItem;
using LevelData;
using ScriptableVariable;
using Unity.Mathematics;
using UnityEngine;

namespace Controllers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private IntVariable _moveAmount;
        [SerializeField] private LevelManager _levelManager;
        [SerializeField] private IntVariable _score;
        [SerializeField] private LevelUIData _levelUIData;
        private Goal firstGoalCopy = new(), secondGoalCopy = new(), thirdGoalCopy = new();
        public const int GoalFruitPoints = 10;
        public const int CommonFruitPoints = 5;
        public float FirstStarPercentage;
        public float SecondStarPercentage;
        public float ThirdStarPercentage;

        public void CopyLevelGoals(Level currentLevel)
        {
            firstGoalCopy = currentLevel.FirstGoal;
            secondGoalCopy = currentLevel.SecondGoal;
            thirdGoalCopy = currentLevel.ThirdGoal;
        }

        private void Start()
        {
            GameEvents.Instance.OnFruitsExplodedEvent.AddListener(UpdateGoalsOnMatch);
            GameEvents.Instance.OnFruitMovedEvent.AddListener(DecreaseMovements);

            SetStarsGoalPercentage();
        }

        private void OnDestroy()
        {
            GameEvents.Instance.OnFruitsExplodedEvent.RemoveListener(UpdateGoalsOnMatch);
            GameEvents.Instance.OnFruitMovedEvent.RemoveListener(DecreaseMovements);
        }

        private void DecreaseMovements() => _moveAmount.Value -= 1;

        public void UpdateGoalsOnMatch(List<Fruit> fruits)
        {
            foreach (Fruit fruit in fruits)
            {
                _levelUIData.UpdateScore(isGoalFruit: TryUpdateGoal(fruit.FruitID.FruitType));
            }

            CheckLevelProgression();
        }

        private bool TryUpdateGoal(FruitType fruitType)
        {
            if (fruitType == firstGoalCopy.FruitID.FruitType)
            {
                int goalNewAmount = Math.Clamp(firstGoalCopy.Amount - 1, 0, firstGoalCopy.Amount);
                firstGoalCopy.Amount = goalNewAmount;
                _levelUIData.UpdateGoalText(firstGoalCopy);
            }
            else if (fruitType == secondGoalCopy.FruitID.FruitType)
            {
                int goalNewAmount = Math.Clamp(secondGoalCopy.Amount - 1, 0, secondGoalCopy.Amount);
                secondGoalCopy.Amount = goalNewAmount;
                _levelUIData.UpdateGoalText(secondGoalCopy);
            }
            else if (fruitType == thirdGoalCopy.FruitID.FruitType)
            {
                int goalNewAmount = Math.Clamp(thirdGoalCopy.Amount - 1, 0, thirdGoalCopy.Amount);
                thirdGoalCopy.Amount = goalNewAmount;
                _levelUIData.UpdateGoalText(thirdGoalCopy);
            }
            else return false;

            return true;
        }

        public void CheckLevelProgression()
        {
            //TODO: Add a bonus to players that finished with movements remaining
            if (!CompletedGoals() && _moveAmount.Value == 0)
            {
                GameEvents.Instance.OnLoseEvent.Invoke();
            }
            else if (CompletedGoals())
            {
                GameEvents.Instance.OnWinEvent.Invoke();
            }
        }

        private bool CompletedGoals()
        {
            return firstGoalCopy.Amount == 0 && secondGoalCopy.Amount == 0 && thirdGoalCopy.Amount == 0;
        }

        public int GetHighestScore()
        {
            int goalAmountSum = firstGoalCopy.Amount + secondGoalCopy.Amount + thirdGoalCopy.Amount;
            int highestScore = goalAmountSum * GoalFruitPoints;

            return highestScore;
        }

        private void SetStarsGoalPercentage()
        {
            FirstStarPercentage = GetHighestScore() * 0.20f;
            SecondStarPercentage = GetHighestScore() * 0.50f;
            ThirdStarPercentage = GetHighestScore() * 1f;
        }
    }
}