using UnityEngine;
using BoardItem;

namespace Board
{
    public class BoardListener : MonoBehaviour
    {
        [SerializeField] private BoardFiller _boardFiller;
        [SerializeField] private BoardSorter _boardSorter;
        [SerializeField] private BoardFruitPool _boardFruitPool;

        public void SubscribeEventsIn(Fruit boardItem)
        {
            int Column = boardItem.Column;
            int Row = boardItem.Row;

            boardItem.OnItemVanish.AddListener((Column, Row) =>
            {
                _boardFiller.ReleaseFruit(Column, Row);
                _boardFruitPool.OnReleasedFruit(boardItem);
                _boardSorter.OnReleasedItem(Column, Row);
            });
        }

        public void UnsubscribeEventsIn(Fruit boardItem)
        {
            boardItem.OnItemVanish.RemoveAllListeners();
        }
    }
}