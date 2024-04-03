using System.Collections.Generic;
using FruitItem;
using Game.UI;
using LevelData;
using ScriptableVariable;
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
                Goal? matchGoal = GetMatchGoal(fruit.FruitID.FruitType);
                if (matchGoal.HasValue)
                {
                    Goal goal = matchGoal.Value;
                    goal.Amount--;

                    _levelUIData.CalculateScore(isGoalFruit: true);
                }

                _levelUIData.CalculateScore(isGoalFruit: false);

            }

            CheckLevelProgression();
        }

        private Goal? GetMatchGoal(FruitType fruitType)
        {
            if (fruitType == firstGoalCopy.FruitID.FruitType) return firstGoalCopy;
            else if (fruitType == secondGoalCopy.FruitID.FruitType) return secondGoalCopy;
            else if (fruitType == thirdGoalCopy.FruitID.FruitType) return thirdGoalCopy;
            else return null;
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