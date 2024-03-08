using UnityEngine;

public class BoardSorter : MonoBehaviour
{
    [SerializeField] private BoardFiller _boardFiller;
    private int BoardRowBound => _boardFiller.Rows;

    public void OnReleasedItem(int emptyRow, int emptyColumn)
    {
        SortColumnStartingAt(emptyRow, emptyColumn);
    }

    private void SortColumnStartingAt(int emptyRow, int emptyColumn)
    {
        int emptySpaces = 0;
        int lastKnownEmptyRow = emptyRow;
        int lastKnownEmptyColumn = emptyColumn;

        for (int i = emptyRow; i < BoardRowBound; i++)
        {
            BoardItem boardItem = _boardFiller.BoardItemsArray[i, emptyColumn];
            if (_boardFiller.IsEmptyBoardItem(i, emptyColumn))
            {
                emptySpaces += 1;
            }
            else
            {
                MoveItemDown(boardItem, emptySpaces);
                lastKnownEmptyRow = i - emptySpaces;
                lastKnownEmptyColumn = emptyColumn;
                emptySpaces = 0;
            }
        }

        _boardFiller.CheckEmptyStartingAt(lastKnownEmptyRow, lastKnownEmptyColumn);
    }

    private void MoveItemDown(BoardItem boardItem, int positions)
    {
        int newRow = boardItem.Row - positions;
        int newColumn = boardItem.Column;
        print(newRow + " _ " + newColumn);
        Vector3 newPosition = _boardFiller.GetItemPosition(boardItem.Row - positions, boardItem.Column);
        _boardFiller.UpdateItemPosition(boardItem, newRow, newColumn, newPosition);
    }
}