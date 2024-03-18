using UnityEngine;
using FruitItem;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

namespace Board
{
    public class BoardMatcher : MonoBehaviour
    {
        [SerializeField] private BoardGrid _boardGrid;
        [SerializeField] private BoardSorter _boardSorter;
        private Vector2Int[] swappedItemsPlacement;

        public IEnumerator MoveFruit(int Column, int Row, Direction direction)
        {
            Vector2Int movementDirection = MovementDirection.GetDirectionCoordinates(direction);

            Vector2Int selectedFruitPosition = new(Column, Row);
            Vector2Int swappedFruitPosition = new(Column + movementDirection.x, Row + movementDirection.y);

            if (!_boardGrid.HasTileAt(swappedFruitPosition.x, swappedFruitPosition.y)) yield break;

            yield return StartCoroutine(SwapFruits(selectedFruitPosition, swappedFruitPosition));
            TryMatchFruits(matchWithMovement: true);
        }

        private IEnumerator SwapFruits(Vector2Int firstFruitPlacement, Vector2Int secondFruitPlacement)
        {
            Fruit selectedFruit = _boardGrid.BoardFruitArray[firstFruitPlacement.x, firstFruitPlacement.y];
            Fruit swappedFruit = _boardGrid.BoardFruitArray[secondFruitPlacement.x, secondFruitPlacement.y];

            swappedItemsPlacement = new Vector2Int[]
            {
                new (selectedFruit.Column, selectedFruit.Row),
                new (swappedFruit.Column, swappedFruit.Row),
            };

            yield return StartCoroutine(_boardSorter.SwapFruitPositions(swappedItemsPlacement[0], swappedItemsPlacement[1]));
        }

        public void TryMatchFruits(bool matchWithMovement)
        {
            List<Fruit> fruitsToMatch = new();

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
                foreach (Fruit fruit in fruitsToMatch)
                {
                    fruit.Vanish();
                }
            }
            else if (fruitsToMatch.Count <= 3 && matchWithMovement)
            {
                StartCoroutine(SwapFruits(swappedItemsPlacement[1], swappedItemsPlacement[0]));
                return;
            }
            else
            {
                return;
            }

            _boardSorter.SortBoard();
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
