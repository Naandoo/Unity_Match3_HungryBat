using UnityEngine;

public class BoardListener : MonoBehaviour
{
    [SerializeField] private BoardFiller _boardFiller;
    [SerializeField] private BoardSorter _boardSorter;
    [SerializeField] private BoardItemPool _boardItemPool;

    public void SubscribeEventsIn(BoardItem boardItem)
    {
        int Column = boardItem.Column;
        int Row = boardItem.Row;

        boardItem.OnItemVanish.AddListener((Column, Row) =>
        {
            _boardFiller.ReleaseItem(Column, Row);
            _boardItemPool.OnReleasedItem(boardItem);
            _boardSorter.OnReleasedItem(Column, Row);
        });
    }

    public void UnsubscribeEventsIn(BoardItem boardItem)
    {
        int Column = boardItem.Column;
        int Row = boardItem.Row;

        boardItem.OnItemVanish.RemoveListener((Column, Row) =>
        {
            _boardFiller.ReleaseItem(Column, Row);
            _boardItemPool.OnReleasedItem(boardItem);
            _boardSorter.OnReleasedItem(Column, Row);
        });
    }
}