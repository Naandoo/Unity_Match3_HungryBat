using UnityEngine;

public class BoardListener : MonoBehaviour
{
    [SerializeField] private BoardFiller _boardFiller;
    [SerializeField] private BoardSorter _boardSorter;
    [SerializeField] private BoardItemPool _boardItemPool;

    public void SubscribeEventsIn(BoardItem boardItem)
    {
        int Row = boardItem.Row;
        int Column = boardItem.Column;

        boardItem.OnItemVanish.AddListener((Row, Column) =>
        {
            _boardFiller.ReleaseItem(Row, Column);
            _boardSorter.OnReleasedItem(Row, Column);
            _boardItemPool.OnReleasedItem(boardItem);
        });
    }

    public void UnsubscribeEvents(BoardItem boardItem)
    {
        int Row = boardItem.Row;
        int Column = boardItem.Column;

        boardItem.OnItemVanish.RemoveListener((Row, Column) =>
        {
            _boardSorter.OnReleasedItem(Row, Column);
            _boardFiller.ReleaseItem(Row, Column);
            _boardItemPool.OnReleasedItem(boardItem);
        });
    }
}