using UnityEngine;
using BoardItem;
using System.Collections.Generic;
using System.Linq;

namespace Board
{
    public class BoardMatcher : MonoBehaviour
    {
        [SerializeField] private BoardGrid _boardGrid;
        private Fruit[] lastSwappedItems;
        private bool lastMovementResultInMatch = false;

        public void MoveFruit(int Column, int Row, Direction direction)
        {
            Vector2Int movementDirection = MovementDirection.GetDirectionCoordinates(direction);
            Vector2Int selectedFruitPosition = new(Column, Row);
            Vector2Int swappedFruitPosition = new(Column + movementDirection.x, Row + movementDirection.y);

            SwapFruits(selectedFruitPosition.x, selectedFruitPosition.y, swappedFruitPosition.x, swappedFruitPosition.y);
            TryMatchFruits();
        }

        private void SwapFruits(int selectedFruitColumn, int selectedFruitRow, int swappedFruitColumn, int swappedFruitRow)
        {
            Fruit selectedFruit = _boardGrid.BoardFruitArray[selectedFruitColumn, selectedFruitRow];
            Fruit swappedFruit = _boardGrid.BoardFruitArray[swappedFruitColumn, swappedFruitRow];

            lastSwappedItems = new Fruit[] { selectedFruit, swappedFruit };

            _boardGrid.BoardFruitArray[selectedFruit.Column, selectedFruit.Row] = swappedFruit;
            _boardGrid.BoardFruitArray[swappedFruit.Column, selectedFruit.Row] = selectedFruit;

            Vector3 selectedFruitPosition = _boardGrid.GetFruitPosition(selectedFruitColumn, selectedFruitRow);
            Vector3 swappedFruitPosition = _boardGrid.GetFruitPosition(swappedFruitColumn, swappedFruitRow);

            selectedFruit.UpdatePosition(swappedFruitColumn, swappedFruitRow, swappedFruitPosition);
            swappedFruit.UpdatePosition(selectedFruitColumn, selectedFruitRow, selectedFruitPosition);

        }

        public void TryMatchFruits()
        {
            bool matched = false;

            do
            {
                matched = false;
                List<Fruit> fruitsToMatch = new List<Fruit>();

                for (int i = 0; i < _boardGrid.Columns; i++)
                {
                    for (int j = 0; j < _boardGrid.Rows; j++)
                    {
                        CheckFruitMatch(i, j, 1, 0, fruitsToMatch);
                        CheckFruitMatch(i, j, 0, 1, fruitsToMatch);
                    }
                }

                if (fruitsToMatch.Count >= 3)
                {
                    matched = true;
                    foreach (Fruit fruit in fruitsToMatch)
                    {
                        fruit.Vanish();
                        lastMovementResultInMatch = true;
                    }
                }

            } while (matched);
        }

        private void CheckFruitMatch(int startColumn, int startRow, int stepX, int stepY, List<Fruit> fruitsToMatch)
        {
            List<Fruit> sequence = new List<Fruit>();
            FruitType? currentFruitType = null;

            for (int i = startColumn, j = startRow; i < _boardGrid.Columns && j < _boardGrid.Rows; i += stepX, j += stepY)
            {
                Fruit fruit = _boardGrid.BoardFruitArray[i, j];

                if (fruit == null)
                {
                    sequence.Clear();
                    currentFruitType = null;
                }
                else if (!currentFruitType.HasValue || currentFruitType == fruit.FruitID.FruitType)
                {
                    sequence.Add(fruit);
                    currentFruitType = fruit.FruitID.FruitType;
                }
                else
                {
                    if (sequence.Count >= 3)
                    {
                        fruitsToMatch.AddRange(sequence);
                    }
                    sequence.Clear();
                    sequence.Add(fruit);
                    currentFruitType = fruit.FruitID.FruitType;
                }
            }

            if (sequence.Count >= 3)
            {
                fruitsToMatch.AddRange(sequence);
            }
        }
    }
}
