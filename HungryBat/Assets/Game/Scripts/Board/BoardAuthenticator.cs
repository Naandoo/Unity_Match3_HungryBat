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
            List<Fruit> bestMatch = new();

            for (int i = 0; i < _boardGrid.Columns; i++)
            {
                for (int j = 0; j < _boardGrid.Rows; j++)
                {
                    if (!_boardGrid.HasTileAt(i, j)) continue;

                    Fruit fruit = _boardGrid.BoardFruitArray[i, j];
                    List<Fruit> matchPossibility = FindBestMatchFromSimulatedPossibilities(fruit);

                    if (matchPossibility.Count > bestMatch.Count)
                    {
                        bestMatch = matchPossibility;
                    }
                }
            }

            if (bestMatch.Count >= 3)
            {
                SaveFruitsForTip(bestMatch);
            }

            return bestMatch.Count >= 3;
        }

        private void SaveFruitsForTip(List<Fruit> fruits)
        {
            foreach (Fruit fruit in fruits)
            {
                StartCoroutine(fruit.Tip());
            }
        }

        private List<Fruit> FindBestMatchFromSimulatedPossibilities(Fruit fruit)
        {
            List<Fruit> bestMatch = new();

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if (direction == Direction.Undefined) continue;

                List<Fruit> match = SimulateMovement(fruit, direction);
                if (match == null) continue;
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

            if (!_boardGrid.HasTileAt(newColumn, newRow)) return null;

            Fruit selectedFruit = _boardGrid.BoardFruitArray[fruit.Column, fruit.Row];
            Fruit swappedFruit = _boardGrid.BoardFruitArray[newColumn, newRow];

            _boardGrid.BoardFruitArray[newColumn, newRow] = selectedFruit;
            _boardGrid.BoardFruitArray[fruit.Column, fruit.Row] = swappedFruit;

            matches.AddRange(GetMatchesFromCell(newColumn, newRow, _boardGrid.BoardFruitArray));

            _boardGrid.BoardFruitArray[fruit.Column, fruit.Row] = selectedFruit;
            _boardGrid.BoardFruitArray[newColumn, newRow] = swappedFruit;

            return matches;
        }

        private List<Fruit> GetMatchesFromCell(int column, int row, Fruit[,] boardFruit)
        {
            List<Fruit> matchesColumn = new();
            List<Fruit> matchesRow = new();

            matchesColumn.AddRange(GetFruitMatch(isColumnCheck: true, index: column, boardFruit));
            matchesRow.AddRange(GetFruitMatch(isColumnCheck: false, index: row, boardFruit));

            return matchesColumn.Count > matchesRow.Count ? matchesColumn : matchesRow;
        }

        public List<Fruit> GetFruitMatch(bool isColumnCheck, int index, Fruit[,] boardFruit)
        {
            List<Fruit> matches = new();
            List<Fruit> sequence = new();

            if (isColumnCheck)
            {
                CheckColumn(index, boardFruit, matches, sequence);
            }
            else
            {
                CheckRow(index, boardFruit, matches, sequence);
            }

            return matches;
        }

        private void CheckColumn(int columnIndex, Fruit[,] boardFruit, List<Fruit> matches, List<Fruit> sequence)
        {
            for (int i = 0; i < _boardGrid.Rows; i++)
            {
                CheckFruit(boardFruit[columnIndex, i], matches, sequence);
            }
        }

        private void CheckRow(int rowIndex, Fruit[,] boardFruit, List<Fruit> matches, List<Fruit> sequence)
        {
            for (int i = 0; i < _boardGrid.Columns; i++)
            {
                CheckFruit(boardFruit[i, rowIndex], matches, sequence);
            }
        }


        private void CheckFruit(Fruit fruit, List<Fruit> matches, List<Fruit> sequence)
        {
            if (fruit == null || !fruit.gameObject.activeSelf) return;

            if (sequence.Count > 0 && sequence[0].FruitID.FruitType == fruit.FruitID.FruitType)
            {
                sequence.Add(fruit);
            }
            else
            {
                if (sequence.Count > matches.Count)
                {
                    matches.Clear();
                    matches.AddRange(sequence);
                }

                sequence.Clear();
                sequence.Add(fruit);
            }
        }
    }
}
