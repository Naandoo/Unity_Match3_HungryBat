using UnityEngine;
using BoardItem;

namespace Board
{
    public class BoardMatcher : MonoBehaviour
    {
        [SerializeField] private BoardGrid _boardGrid;
        //board matcher listens to it and simulate movement, checking merges, if possible move the fruit, else do a trick animation
        public void TryMatchFruit(int Column, int Row, Direction direction)
        {
            EqualFruitsCount equalFruitsCount = GetEqualFruitsCountStartingAt(Column, Row);
            //call method that handle the match possibilities based on the merge count
        }

        private EqualFruitsCount GetEqualFruitsCountStartingAt(int Column, int Row)
        {
            EqualFruitsCount equalFruitsCount = new();
            FruitType fruitType = _boardGrid.BoardFruitArray[Column, Row].FruitID.FruitType;

            equalFruitsCount.left = GetFruitsCountInDirection(Column, Row, fruitType, -1, 0);
            equalFruitsCount.right = GetFruitsCountInDirection(Column, Row, fruitType, 1, 0);
            equalFruitsCount.up = GetFruitsCountInDirection(Column, Row, fruitType, 0, -1);
            equalFruitsCount.down = GetFruitsCountInDirection(Column, Row, fruitType, 0, 1);

            return equalFruitsCount;
        }

        private int GetFruitsCountInDirection(int startColumn, int startRow,
        FruitType selectedFruitType, int columnDirection, int rowDirection)
        {
            int amountOfFruits = 0;

            int currentColumn = startColumn + columnDirection;
            int currentRow = startRow + rowDirection;

            while (IsValidPosition(currentColumn, currentRow) &&
            _boardGrid.BoardFruitArray[currentColumn, currentRow].FruitID.FruitType == selectedFruitType)
            {
                amountOfFruits++;
                currentColumn += columnDirection;
                currentColumn += rowDirection;
            }

            return amountOfFruits;
        }

        private bool IsValidPosition(int column, int row)
        {
            return column >= 0 && column < _boardGrid.Columns && row >= 0 && row < _boardGrid.Rows;
        }
    }

    public class EqualFruitsCount
    {
        public int left = 0;
        public int right = 0;
        public int up = 0;
        public int down = 0;
    }
}