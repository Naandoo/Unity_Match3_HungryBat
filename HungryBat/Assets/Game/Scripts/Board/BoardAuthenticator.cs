using System.Collections.Generic;
using FruitItem;
using UnityEngine;
using System;

namespace Board
{
    public class BoardAuthenticator : MonoBehaviour
    {
        [SerializeField] private BoardGrid _boardGrid;
        [SerializeField] private BoardMatcher _boardMatcher;

        public bool ContainsAvailableMatches()
        {
            List<Fruit> match = new();

            for (int i = 0; i < _boardGrid.Columns; i++)
            {
                for (int j = 0; j < _boardGrid.Rows; j++)
                {
                    if (!_boardGrid.HasTileAt(i, j)) continue;

                    Fruit fruit = _boardGrid.BoardFruitArray[i, j];
                    List<Fruit> matchPossibility = FindBestMatchFromSimulatedPossibilities(fruit);
                    if (matchPossibility.Count >= match.Count)
                    {
                        match = matchPossibility;
                    }
                }
            }

            if (match.Count >= 3)
            {
                SaveFruitsForTip(match);
            }

            return match.Count >= 3;
        }

        private List<Fruit> FindBestMatchFromSimulatedPossibilities(Fruit fruit)
        {
            List<Fruit> bestMatch = new();

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                List<Fruit> match = SimulateMovement(fruit, direction);
                if (match.Count > bestMatch.Count)
                {
                    bestMatch = match;
                }
            }

            return bestMatch;
        }

        private List<Fruit> SimulateMovement(Fruit fruit, Direction direction)
        {
            List<Fruit> matches = new();

            Vector2Int movementDirection = MovementDirection.GetDirectionCoordinates(direction);
            int newColumn = fruit.Column + movementDirection.x;
            int newRow = fruit.Row + movementDirection.y;

            if (!_boardGrid.HasTileAt(newColumn, newRow)) return matches;

            Fruit[,] tempBoardFruit = _boardGrid.CloneBoardFruitArray();

            Fruit selectedFruit = tempBoardFruit[fruit.Column, fruit.Row];
            Fruit swappedFruit = tempBoardFruit[newColumn, newRow];

            tempBoardFruit[newColumn, newRow] = selectedFruit;
            tempBoardFruit[fruit.Column, fruit.Row] = swappedFruit;

            matches.AddRange(GetMatchesFromCell(newColumn, newRow, tempBoardFruit));

            return matches;
        }

        private List<Fruit> GetMatchesFromCell(int column, int row, Fruit[,] boardFruit)
        {
            List<Fruit> matches = new();

            matches.AddRange(_boardMatcher.GetFruitMatch(column, row, 1, 0, boardFruit));
            matches.AddRange(_boardMatcher.GetFruitMatch(column, row, 0, 1, boardFruit));

            return matches;
        }

        private void SaveFruitsForTip(List<Fruit> fruits)
        {
            foreach (Fruit fruit in fruits)
            {
                StartCoroutine(fruit.Tip());
            }
        }
    }
}
