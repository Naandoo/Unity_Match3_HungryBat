using UnityEngine;
using FruitItem;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using ScriptableVariable;
namespace Board
{
    public class BoardMatcher : MonoBehaviour
    {
        [SerializeField] private BoardGrid _boardGrid;
        [SerializeField] private BoardSorter _boardSorter;
        [SerializeField] private BoardAuthenticator _boardAuthenticator;
        [SerializeField] private BoolVariable _isLevelFinished;
        private int _boardColumns;
        private int _boardRows;
        private Vector2Int[] _swappedItemsPlacement;

        private void Awake()
        {
            _boardColumns = _boardGrid.Columns;
            _boardRows = _boardGrid.Rows;
        }

        public IEnumerator MoveFruit(int column, int row, Direction direction)
        {
            Vector2Int movementDirection = MovementDirection.GetDirectionCoordinates(direction);

            Vector2Int selectedFruitPosition = new(column, row);
            Vector2Int swappedFruitPosition = new(column + movementDirection.x, row + movementDirection.y);

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
                new (selectedFruit.Column, selectedFruit.Row),
                new (swappedFruit.Column, swappedFruit.Row),
            };

            yield return StartCoroutine(_boardSorter.SwapFruitPositions(_swappedItemsPlacement[0], _swappedItemsPlacement[1]));
        }

        public void TryMatchFruits(bool matchWithMovement)
        {
            List<Fruit> fruitsToMatch = GetAllMatchesInBoard();

            if (fruitsToMatch.Count >= 3)
            {
                foreach (Fruit fruit in fruitsToMatch)
                {
                    fruit.Vanish();
                }

                if (matchWithMovement) GameEvents.Instance.OnFruitMovedEvent.Invoke();
                if (!_isLevelFinished.Value) GameEvents.Instance.OnFruitsExplodedEvent.Invoke(fruitsToMatch);

            }
            else if (fruitsToMatch.Count <= 3 && matchWithMovement)
            {
                StartCoroutine(SwapFruits(_swappedItemsPlacement[1], _swappedItemsPlacement[0]));
                BoardState.Instance.SetState(State.Common);
                return;
            }
            else
            {
                BoardState.Instance.SetState(State.Common);
                if (!_isLevelFinished.Value) GameEvents.Instance.OnBoardFinishMovement.Invoke();
                return;
            }

            StartCoroutine(_boardSorter.SortBoard());
        }

        public List<Fruit> GetAllMatchesInBoard()
        {
            List<Fruit> fruitsToMatch = new();

            //TODO: Check the impact of this algorithm in performance and consider apply changes
            for (int i = 0; i < _boardColumns; i++)
            {
                for (int j = 0; j < _boardRows; j++)
                {
                    List<Fruit> horizontalMatch = GetBoardMatch(i, j, 1, 0);
                    List<Fruit> verticalMatch = GetBoardMatch(i, j, 0, 1);

                    foreach (Fruit fruit in horizontalMatch)
                    {
                        if (!fruitsToMatch.Contains(fruit))
                        {
                            fruitsToMatch.Add(fruit);
                        }
                    }

                    foreach (Fruit fruit in verticalMatch)
                    {
                        if (!fruitsToMatch.Contains(fruit))
                        {
                            fruitsToMatch.Add(fruit);
                        }
                    }
                }
            }

            return fruitsToMatch;
        }
        private List<Fruit> GetBoardMatch(int startColumn, int startRow, int stepX, int stepY)
        {
            return GetFruitMatch(startColumn, startRow, stepX, stepY, _boardGrid.BoardFruitArray);
        }

        public List<Fruit> GetFruitMatch(int startColumn, int startRow, int stepX, int stepY, Fruit[,] boardFruit)
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
