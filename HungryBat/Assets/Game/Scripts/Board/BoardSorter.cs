using System.Data;
using UnityEngine;

public class BoardSorter : MonoBehaviour
{
    [SerializeField] private BoardFiller _boardFiller;
    private int BoardRowBound => _boardFiller.Rows;

    public void OnReleasedItem(int emptyColumn, int emptyRow)
    {
        SortColumnStartingAt(emptyColumn, emptyRow);
    }

    private void SortColumnStartingAt(int emptyColumn, int emptyRow)
    {
        int emptySpaces = 0;
        int lastKnownEmptyColumn = emptyColumn;
        int lastKnownEmptyRow = emptyRow;

        for (int i = emptyRow; i < BoardRowBound; i++)
        {
            if (!_boardFiller.HasTileAt(emptyColumn, i)) continue;
            if (_boardFiller.IsEmptyBoardItem(emptyColumn, i))
            {
                emptySpaces += 1;
            }
            else
            {
                BoardItem boardItemAbove = _boardFiller.BoardItemsArray[emptyColumn, i];
                MoveItemDown(boardItemAbove, emptySpaces);

                lastKnownEmptyRow = i;
                lastKnownEmptyColumn = emptyColumn;
            }
        }

        _boardFiller.CheckEmptyStartingAt(lastKnownEmptyColumn, lastKnownEmptyRow);
    }

    private void MoveItemDown(BoardItem boardItemAbove, int emptyPositions)
    {
        int newColumn = boardItemAbove.Column;
        int newRow = boardItemAbove.Row - emptyPositions;
        Vector3 newPosition = _boardFiller.GetItemPosition(newColumn, newRow);
        _boardFiller.UpdateItemPosition(boardItemAbove, newColumn, newRow, newPosition);
    }
}