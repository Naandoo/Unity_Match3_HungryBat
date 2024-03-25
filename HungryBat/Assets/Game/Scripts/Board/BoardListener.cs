using UnityEngine;
using FruitItem;

namespace Board
{
    public class BoardListener : MonoBehaviour
    {
        [SerializeField] private BoardGrid _boardGrid;
        [SerializeField] private BoardFruitPool _boardFruitPool;
        [SerializeField] private BoardMatcher _boardMatcher;
        [SerializeField] private BoardState _boardState;


        private void Start()
        {
            _boardMatcher.OnBoardFinishMovement.AddListener(() =>
          {
              _boardGrid.CheckShuffleNeed();
          });
        }

        public void SubscribeEventsIn(Fruit boardItem)
        {
            int Column = boardItem.Column;
            int Row = boardItem.Row;

            if (boardItem.OnItemVanish.GetPersistentEventCount() == 0)
            {
                boardItem.OnItemVanish.AddListener((Column, Row) =>
                {
                    _boardGrid.ReleaseFruit(Column, Row);
                    _boardFruitPool.OnReleasedFruit(boardItem);
                });
            }

            if (boardItem.OnItemMoved.GetPersistentEventCount() == 0)
            {
                boardItem.OnItemMoved.AddListener((Column, Row, lastMoveDirection) =>
                {
                    if (_boardState.State == State.Common)
                    {
                        StartCoroutine(_boardMatcher.MoveFruit(Column, Row, lastMoveDirection));
                    }
                });
            }
        }

        public void UnsubscribeEventsIn(Fruit boardItem)
        {
            boardItem.OnItemVanish.RemoveAllListeners();
            boardItem.OnItemMoved.RemoveAllListeners();
        }
    }
}