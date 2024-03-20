using UnityEngine;
using FruitItem;
using System.Collections;

namespace Board
{
    public class BoardSorter : MonoBehaviour
    {
        [SerializeField] private BoardGrid _boardGrid;
        [SerializeField] private BoardMatcher _boardMatcher;
        [SerializeField] private BoardState _boardState;

        public void SortBoard()
        {
            _boardState.State = State.Sorting;

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

                StartCoroutine(FillEmptySpacesInBoard(column: i, emptyItemsInColumn));
            }
        }

        public bool IsEmptyBoardItem(int column, int row)
        {
            bool IsEmpty = _boardGrid.BoardFruitArray[column, row] == null;
            return IsEmpty;
        }

        private void MoveItem(Fruit boardFruit, int distance, Direction direction)
        {
            int Column = boardFruit.Column;
            int Row = boardFruit.Row;

            switch (direction)
            {
                case Direction.Up:
                    Row += distance;
                    break;
                case Direction.Down:
                    Row -= distance;
                    break;
                case Direction.Right:
                    Column += distance;
                    break;
                case Direction.Left:
                    Column -= distance;
                    break;
            }

            StartCoroutine(UpdateFruitPosition(boardFruit, Column, Row));
        }

        private IEnumerator UpdateFruitPosition(Fruit boardFruit, int newColumn, int newRow)
        {
            Vector3 newPosition = _boardGrid.GetFruitPosition(newColumn, newRow);
            _boardGrid.BoardFruitArray[newColumn, newRow] = boardFruit;

            yield return StartCoroutine(boardFruit.UpdatePosition(newColumn, newRow, newPosition));
        }

        private IEnumerator FillEmptySpacesInBoard(int column, int emptyItems)
        {
            int initialRow = GetRowsInColumn(column, out int firstRowIndex) - emptyItems;

            for (int i = initialRow + firstRowIndex; i < _boardGrid.Rows; i++)
            {
                if (!_boardGrid.HasTileAt(column, i)) continue;
                _boardGrid.GenerateBoardFruit(column, i);
            }

            yield return new WaitForSeconds(0.6f);
            _boardMatcher.TryMatchFruits(matchWithMovement: false);
        }

        private int GetRowsInColumn(int column, out int firstRowIndex)
        {
            int amountOfEmptyTiles = 0;
            bool foundFirstRowIndex = false;
            firstRowIndex = 0;

            for (int i = 0; i < _boardGrid.Rows; i++)
            {
                if (!_boardGrid.HasTileAt(column, i))
                {
                    amountOfEmptyTiles++;
                }
                else
                {
                    if (!foundFirstRowIndex)
                    {
                        firstRowIndex = i;
                        foundFirstRowIndex = true;
                    }
                }
            }

            int amountOfRowsInColumn = _boardGrid.Rows - amountOfEmptyTiles;
            return amountOfRowsInColumn;
        }
        public IEnumerator SwapFruitPositions(Vector2Int firstFruitPlacement, Vector2Int secondFruitPlacement)
        {
            _boardState.State = State.Moving;

            Fruit firstFruit = _boardGrid.BoardFruitArray[firstFruitPlacement.x, firstFruitPlacement.y];
            Fruit secondFruit = _boardGrid.BoardFruitArray[secondFruitPlacement.x, secondFruitPlacement.y];

            StartCoroutine(UpdateFruitPosition(firstFruit, secondFruitPlacement.x, secondFruitPlacement.y));
            yield return StartCoroutine(UpdateFruitPosition(secondFruit, firstFruitPlacement.x, firstFruitPlacement.y));
        }
    }
}
