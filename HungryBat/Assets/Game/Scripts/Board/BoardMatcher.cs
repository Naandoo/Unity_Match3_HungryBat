using UnityEngine;
using FruitItem;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using System;

namespace Board
{
    public class BoardMatcher : MonoBehaviour
    {
        [SerializeField] private BoardGrid _boardGrid;
        [SerializeField] private BoardSorter _boardSorter;
        [SerializeField] private BoardState _boardState;
        [SerializeField] private BoardAuthenticator _boardAuthenticator;

        private Vector2Int[] swappedItemsPlacement;
        public UnityEvent OnBoardFinishMovement;

        public IEnumerator MoveFruit(int Column, int Row, Direction direction)
        {
            Vector2Int movementDirection = MovementDirection.GetDirectionCoordinates(direction);

            Vector2Int selectedFruitPosition = new(Column, Row);
            Vector2Int swappedFruitPosition = new(Column + movementDirection.x, Row + movementDirection.y);

            if (!_boardGrid.HasTileAt(swappedFruitPosition.x, swappedFruitPosition.y)) yield break;

            yield return StartCoroutine(SwapFruits(selectedFruitPosition, swappedFruitPosition));
            MatchFruits(matchWithMovement: true);
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

        public void MatchFruits(bool matchWithMovement)
        {
            Match fruitsToMatch = new();

            for (int i = 0; i < _boardGrid.Columns; i++)
            {
                for (int j = 0; j < _boardGrid.Rows; j++)
                {
                    fruitsToMatch.Clone(GetBoardMatch(i, j));
                }
            }

            if (fruitsToMatch.Count >= 3)
            {
                MatchFruits(fruitsToMatch);
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
                // OnBoardFinishMovement.Invoke();
                return;
            }

            StartCoroutine(_boardSorter.SortBoard());
        }

        private Match GetBoardMatch(int startColumn, int startRow)
        {
            Match matches = _boardAuthenticator.GetFruitMatch(startColumn, startRow, _boardGrid.BoardFruitArray);

            return matches;
        }

        private void MatchFruits(Match match)
        {
            Fruit[,] boardFruit = _boardGrid.BoardFruitArray;
            Vector2Int startingPosition = match.StartingPosition;

            for (int i = 0; i < match.LeftCount; i++)
            {
                boardFruit[startingPosition.x - i, startingPosition.y].Vanish();
            }
            for (int i = 0; i < match.RightCount; i++)
            {
                boardFruit[startingPosition.x + i, startingPosition.y].Vanish();
            }
            for (int i = 0; i < match.AboveCount; i++)
            {
                boardFruit[startingPosition.x, startingPosition.y + i].Vanish();
            }
            for (int i = 0; i < match.BelowCount; i++)
            {
                boardFruit[startingPosition.x, startingPosition.y - i].Vanish();
            }

            boardFruit[startingPosition.x, startingPosition.y].Vanish();
        }
    }
}
