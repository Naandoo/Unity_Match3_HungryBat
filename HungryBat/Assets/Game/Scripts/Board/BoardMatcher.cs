using UnityEngine;
using FruitItem;
using System.Collections.Generic;
using System.Collections;
using Board;

namespace Board
{

    public class BoardMatcher : MonoBehaviour
    {
        [SerializeField] private BoardGrid _boardGrid;
        [SerializeField] private BoardSorter _boardSorter;
        [SerializeField] private BoardAuthenticator _boardAuthenticator;
        [SerializeField] private BoardState _boardState;
        private int _boardColumns;
        private int _boardRows;

        private Vector2Int[] _swappedItemsPlacement;

        private void Awake()
        {
            _boardColumns = _boardGrid.Columns;
            _boardRows = _boardGrid.Rows;
        }

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

        public IEnumerator MoveFruit(int column, int row, Direction direction)
        {
            Vector2Int movementDirection = MovementDirection.GetDirectionCoordinates(direction);

            Vector2Int selectedFruitPosition = new Vector2Int(column, row);
            Vector2Int swappedFruitPosition = new Vector2Int(column + movementDirection.x, row + movementDirection.y);

            if (!_boardGrid.HasTileAt(swappedFruitPosition.x, swappedFruitPosition.y)) yield break;

            yield return StartCoroutine(SwapFruits(selectedFruitPosition, swappedFruitPosition));
            TryMatchFruits(matchWithMovement: true);
        }

        private IEnumerator SwapFruits(Vector2Int firstFruitPlacement, Vector2Int secondFruitPlacement)
        {
            Fruit selectedFruit = _boardGrid.BoardFruitArray[firstFruitPlacement.x, firstFruitPlacement.y];
            Fruit swappedFruit = _boardGrid.BoardFruitArray[secondFruitPlacement.x, secondFruitPlacement.y];

            _swappedItemsPlacement = new Vector2Int[]
            {
                new Vector2Int(selectedFruit.Column, selectedFruit.Row),
                new Vector2Int(swappedFruit.Column, swappedFruit.Row),
            };

            yield return StartCoroutine(_boardSorter.SwapFruitPositions(_swappedItemsPlacement[0], _swappedItemsPlacement[1]));
        }

        public void TryMatchFruits(bool matchWithMovement)
        {
            List<Fruit> fruitsToMatch = new();

            for (int i = 0; i < _boardColumns; i++)
            {
                for (int j = 0; j < _boardRows; j++)
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
                StartCoroutine(SwapFruits(_swappedItemsPlacement[1], _swappedItemsPlacement[0]));
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
            return GetFruitMatch(startColumn, startRow, stepX, stepY, _boardGrid.BoardFruitArray);
        }

        private List<Fruit> GetFruitMatch(int startColumn, int startRow, int stepX, int stepY, Fruit[,] boardFruit)
        {
            List<Fruit> matches = new();
            List<Fruit> sequence = new();

            FruitType? currentFruitType = null;

            for (int i = startColumn, j = startRow; i >= 0 && i < _boardColumns && j >= 0 && j < _boardRows; i += stepX, j += stepY)
            {
                if (!boardFruit[i, j] || !boardFruit[i, j].gameObject.activeSelf) continue;

                Fruit fruit = boardFruit[i, j];

                if (!currentFruitType.HasValue || currentFruitType == fruit.FruitID.FruitType)
                {
                    sequence.Add(fruit);
                    currentFruitType = fruit.FruitID.FruitType;

                    if (sequence.Count >= 3)
                    {
                        if (sequence.Count > matches.Count)
                        {
                            matches.Clear();
                            matches.AddRange(sequence);
                        }
                    }
                }
                else
                {
                    sequence.Clear();
                    sequence.Add(fruit);
                    currentFruitType = fruit.FruitID.FruitType;
                }
            }

            return matches;
        }
    }
}
