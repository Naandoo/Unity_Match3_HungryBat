using UnityEngine;
using BoardItem;

namespace Board
{
    public class BoardSorter : MonoBehaviour
    {
        [SerializeField] private BoardGrid _boardGrid;
        private int BoardRowBound => _boardGrid.Rows;

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
                if (!_boardGrid.HasTileAt(emptyColumn, i)) continue;
                if (IsEmptyBoardItem(emptyColumn, i))
                {
                    emptySpaces += 1;
                }
                else
                {
                    Fruit boardItem = _boardGrid.BoardFruitArray[emptyColumn, i];
                    MoveItem(boardItem, emptySpaces, Direction.Down);

                    lastKnownEmptyRow = i;
                    lastKnownEmptyColumn = emptyColumn;
                }
            }

            CheckEmptyStartingAt(lastKnownEmptyColumn, lastKnownEmptyRow);
        }

        public void CheckEmptyStartingAt(int column, int row)
        {
            for (int i = row; i < _boardGrid.Rows; i++)
            {
                if (IsEmptyBoardItem(column, row))
                {
                    _boardGrid.GenerateBoardFruit(column, row: i);
                }
            }
        }

        public bool IsEmptyBoardItem(int column, int row)
        {
            bool IsEmpty = _boardGrid.BoardFruitArray[column, row] == null;
            return IsEmpty;
        }

        private void MoveItem(Fruit boardFruit, int distance, Direction direction)
        {
            int newColumn = boardFruit.Column;
            int newRow = boardFruit.Row;

            switch (direction)
            {
                case Direction.Up:
                    newRow += distance;
                    break;
                case Direction.Down:
                    newRow -= distance;
                    break;
                case Direction.Right:
                    newColumn += distance;
                    break;
                case Direction.Left:
                    newColumn -= distance;
                    break;
            }

            Vector3 newPosition = _boardGrid.GetFruitPosition(newColumn, newRow);
            UpdateFruitPosition(boardFruit, newColumn, newRow, newPosition);
        }

        public void UpdateFruitPosition(Fruit boardFruit, int newColumn, int newRow, Vector3 itemPosition)
        {
            int oldColumn = boardFruit.Column;
            int oldRow = boardFruit.Row;

            _boardGrid.BoardFruitArray[newColumn, newRow] = boardFruit;
            boardFruit.UpdatePosition(newColumn, newRow, itemPosition);

            _boardGrid.ReleaseFruit(oldColumn, oldRow);
        }
    }
}
