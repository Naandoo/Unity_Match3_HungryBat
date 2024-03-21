using UnityEngine;
using FruitItem;
using System.Collections.Generic;
using System.Collections;

namespace Board
{
    public class BoardMatcher : MonoBehaviour
    {
        [SerializeField] private BoardGrid _boardGrid;
        [SerializeField] private BoardSorter _boardSorter;
        [SerializeField] private BoardState _boardState;
        [SerializeField] private BoardAuthenticator _boardAuthenticator;

        private Vector2Int[] swappedItemsPlacement;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                bool hasMatch;
                List<Fruit> matches;
                hasMatch = _boardAuthenticator.VerifyAvailableMatches(_boardGrid.BoardFruitArray, out matches);

                foreach (Fruit fruit in matches)
                {
                    fruit.transform.localScale *= 1.05f;
                }
            }
        }
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
                    fruitsToMatch.AddRange(GetBoardMatch(i, j, 1, 0));
                    fruitsToMatch.AddRange(GetBoardMatch(i, j, 0, 1));
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
                _boardState.State = State.Common;
                return;
            }
            else
            {
                _boardState.State = State.Common;
                return;
            }

            _boardSorter.SortBoard();
        }

        private List<Fruit> GetBoardMatch(int startColumn, int startRow, int stepX, int stepY)
        {
            List<Fruit> matches = new();
            matches = _boardAuthenticator.GetFruitMatch(startColumn, startRow, stepX, stepY, _boardGrid.BoardFruitArray);

            return matches;
        }
    }
}
