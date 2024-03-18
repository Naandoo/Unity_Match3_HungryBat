using UnityEngine;
using FruitItem;
using System.Collections;
using Unity.VisualScripting;

namespace Board
{
    public class BoardSorter : MonoBehaviour
    {
        [SerializeField] private BoardGrid _boardGrid;
        [SerializeField] private BoardMatcher _boardMatcher;

        public void SortBoard()
        {
            for (int i = 0; i < _boardGrid.Columns; i++)
            {
                int emptyItemsInColumn = 0;

                for (int j = 0; j < _boardGrid.Rows; j++)
                {
                    if (!_boardGrid.HasTileAt(i, j)) continue;
                    if (IsEmptyBoardItem(i, j))
                    {
                        emptyItemsInColumn += 1;
                    }
                    else
                    {
                        Fruit fruit = _boardGrid.BoardFruitArray[i, j];
                        MoveItem(fruit, emptyItemsInColumn, Direction.Down);
                    }
                }

                FillEmptySpacesInBoard(column: i, emptyItemsInColumn);
            }
        }

        private void FillEmptySpacesInBoard(int column, int emptyItems)
        {
            int initialRow = _boardGrid.Rows - emptyItems;

            for (int i = initialRow; i < _boardGrid.Rows; i++)
            {
                int newRow;
                if (!_boardGrid.HasTileAt(column, i))
                {
                    newRow = i - 1;
                }
                else newRow = i;
                _boardGrid.GenerateBoardFruit(column, newRow);
            }

            _boardMatcher.TryMatchFruits(matchWithMovement: false);
        }
        private void MoveItem(Fruit boardFruit, int distance, Direction direction)
        {
            int Column = boardFruit.Column;
            int Row = boardFruit.Row;

            switch (direction)
            {
                case Direction.Up:
                    Row = Mathf.Clamp(Row += distance, 0, _boardGrid.Rows);
                    break;
                case Direction.Down:
                    Row = Mathf.Clamp(Row -= distance, 0, _boardGrid.Rows);
                    break;
                case Direction.Right:
                    Column = Mathf.Clamp(Column += distance, 0, _boardGrid.Columns);
                    break;
                case Direction.Left:
                    Column = Mathf.Clamp(Column -= distance, 0, _boardGrid.Columns);
                    break;
            }

            StartCoroutine(UpdateFruitPosition(boardFruit, Column, Row));
        }

        public bool IsEmptyBoardItem(int column, int row)
        {
            bool IsEmpty = _boardGrid.BoardFruitArray[column, row] == null;
            return IsEmpty;
        }



        private IEnumerator UpdateFruitPosition(Fruit boardFruit, int newColumn, int newRow)
        {
            Vector3 newPosition = _boardGrid.GetFruitPosition(newColumn, newRow);
            _boardGrid.BoardFruitArray[newColumn, newRow] = boardFruit;

            yield return StartCoroutine(boardFruit.UpdatePosition(newColumn, newRow, newPosition));
        }

        public IEnumerator SwapFruitPositions(Vector2Int firstFruitPlacement, Vector2Int secondFruitPlacement)
        {
            Fruit firstFruit = _boardGrid.BoardFruitArray[firstFruitPlacement.x, firstFruitPlacement.y];
            Fruit secondFruit = _boardGrid.BoardFruitArray[secondFruitPlacement.x, secondFruitPlacement.y];

            StartCoroutine(UpdateFruitPosition(firstFruit, secondFruitPlacement.x, secondFruitPlacement.y));
            yield return StartCoroutine(UpdateFruitPosition(secondFruit, firstFruitPlacement.x, firstFruitPlacement.y));
        }
    }
}
