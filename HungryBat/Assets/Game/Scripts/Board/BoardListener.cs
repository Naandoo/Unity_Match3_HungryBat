using UnityEngine;
using FruitItem;

namespace Board
{
    public class BoardListener : MonoBehaviour
    {
        [SerializeField] private BoardGrid _boardGrid;
        [SerializeField] private BoardFruitPool _boardFruitPool;
        [SerializeField] private BoardMatcher _boardMatcher;

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
                    if (BoardState.Instance.State == State.Common)
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
        }

        public void UnsubscribeEventsIn(Fruit fruit)
        {
            fruit.OnItemVanish.RemoveAllListeners();
            fruit.OnItemMoved.RemoveAllListeners();
            BoardState.Instance.onStateChange.RemoveListener((state) =>
            {
                fruit.EndTipRoutine();
            });
        }
    }
}