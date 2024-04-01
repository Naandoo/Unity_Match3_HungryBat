using UnityEngine;
using FruitItem;
using Skill;
using ScriptableVariable;

namespace Board
{
    public class BoardSubscriber : MonoBehaviour
    {
        [SerializeField] private BoardGrid _boardGrid;
        [SerializeField] private BoardFruitPool _boardFruitPool;
        [SerializeField] private BoardMatcher _boardMatcher;
        [SerializeField] private SkillManager _skillManager;
        [SerializeField] private IntVariable _movesAmount;

        private void Awake()
        {
            _boardMatcher.OnBoardFinishMovement.AddListener(() =>
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
                    _boardFruitPool.OnReleasedFruit(fruit);
                });
            }

            if (fruit.OnItemMoved.GetPersistentEventCount() == 0)
            {
                fruit.OnItemMoved.AddListener((Column, Row, lastMoveDirection) =>
                {
                    if (BoardState.Instance.State == State.Common && _movesAmount.Value > 0)
                    {
                        StartCoroutine(_boardMatcher.MoveFruit(Column, Row, lastMoveDirection));
                    }
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
                    StartCoroutine(_skillManager.CheckSkillState(fruit));
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