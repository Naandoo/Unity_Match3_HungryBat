using UnityEngine;
using FruitItem;
using Skills;
using ScriptableVariables;
using Controllers;

namespace Board
{
    public class BoardSubscriber : MonoBehaviour
    {
        [SerializeField] private BoardGrid _boardGrid;
        [SerializeField] private BoardFruitPool _boardFruitPool;
        [SerializeField] private BoardMatcher _boardMatcher;
        [SerializeField] private SkillManager _skillManager;
        [SerializeField] private IntVariable _movesAmount;
        [SerializeField] private BoolVariable _isLevelFinished;

        private void Start()
        {
            GameEvents.Instance.OnBoardFinishMovement.AddListener(() =>
            {
                _boardGrid.CheckShuffleNeed();
            });
        }

        private void OnDestroy()
        {
            GameEvents.Instance.OnBoardFinishMovement.RemoveListener(() =>
            {
                _boardGrid.CheckShuffleNeed();
            });
        }

        public void SubscribeEventsIn(Fruit fruit)
        {
            int Column = fruit.Column;
            int Row = fruit.Row;

            if (fruit.OnItemVanish.GetPersistentEventCount() == 0)
            {
                fruit.OnItemVanish.AddListener((Column, Row) =>
                {
                    _boardGrid.ReleaseFruit(Column, Row);
                    _boardFruitPool.ReleaseFruit(fruit);
                });
            }

            if (fruit.OnItemMoved.GetPersistentEventCount() == 0)
            {
                fruit.OnItemMoved.AddListener((Column, Row, lastMoveDirection) =>
                {
                    StartCoroutine(_boardMatcher.MoveFruit(Column, Row, lastMoveDirection));
                });
            }

            if (fruit.OnItemMoved.GetPersistentEventCount() == 0)
            {
                BoardState.Instance.onStateChange.AddListener((state) =>
                {
                    fruit.EndTipRoutine();
                });
            }

            if (fruit.onSelectedFruit.GetPersistentEventCount() == 0)
            {
                fruit.onSelectedFruit.AddListener((fruit) =>
                {
                    StartCoroutine(_skillManager.TriggerSkillOn(fruit));
                });
            }

        }

        public void UnsubscribeEventsIn(Fruit fruit)
        {
            fruit.OnItemVanish.RemoveAllListeners();
            fruit.OnItemMoved.RemoveAllListeners();
            fruit.onSelectedFruit.RemoveAllListeners();

            BoardState.Instance.onStateChange.RemoveListener((state) =>
            {
                fruit.EndTipRoutine();
            });
        }
    }
}