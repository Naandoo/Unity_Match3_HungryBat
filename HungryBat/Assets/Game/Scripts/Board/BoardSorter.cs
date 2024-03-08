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
            BoardItem boardItem = _boardFiller.BoardItemsArray[emptyColumn, i];
            if (_boardFiller.IsEmptyBoardItem(emptyColumn, i))
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

        _boardFiller.CheckEmptyStartingAt(lastKnownEmptyColumn, lastKnownEmptyColumn);
    }

    private void MoveItemDown(BoardItem boardItem, int positions)
    {
        int newColumn = boardItem.Column;
        int newRow = boardItem.Row - positions;
        print(newColumn + " _ " + newRow);
        Vector3 newPosition = _boardFiller.GetItemPosition(boardItem.Column, boardItem.Row - positions);
        _boardFiller.UpdateItemPosition(boardItem, newColumn, newRow, newPosition);
    }
}