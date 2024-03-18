using UnityEngine;
using FruitItem;

namespace Board
{
    public class BoardListener : MonoBehaviour
    {
        [SerializeField] private BoardGrid _boardGrid;
        [SerializeField] private BoardSorter _boardSorter;
        [SerializeField] private BoardFruitPool _boardFruitPool;
        [SerializeField] private BoardMatcher _boardMatcher;

        public void SubscribeEventsIn(Fruit boardItem)
        {
            int Column = boardItem.Column;
            int Row = boardItem.Row;

            boardItem.OnItemVanish.AddListener((Column, Row) =>
            {
                _boardGrid.ReleaseFruit(Column, Row);
                _boardFruitPool.OnReleasedFruit(boardItem);
                // _boardSorter.OnReleasedItem(Column, Row);
            });

            boardItem.OnItemMoved.AddListener((Column, Row, lastMoveDirection) =>
            {
                StartCoroutine(_boardMatcher.MoveFruit(Column, Row, lastMoveDirection));
            });
        }

        public void UnsubscribeEventsIn(Fruit boardItem)
        {
            boardItem.OnItemVanish.RemoveAllListeners();
            boardItem.OnItemMoved.RemoveAllListeners();
        }
    }
}