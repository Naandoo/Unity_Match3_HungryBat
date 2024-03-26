using System.Collections.Generic;
using FruitItem;
using UnityEngine;
using System;

namespace Board
{
    public class BoardAuthenticator : MonoBehaviour
    {
        [SerializeField] BoardGrid _boardGrid;

        public bool VerifyAvailableMatches(Fruit[,] boardFruits, out List<Fruit> match)
        {
            match = new List<Fruit>();

            for (int i = 0; i < boardFruits.GetLength(0); i++)
            {
                for (int j = 0; j < boardFruits.GetLength(1); j++)
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

            return match.Count >= 3;
        }


        private List<Fruit> FindBestMatchFromSimulatedPossibilities(Fruit fruit)
        {
            List<Fruit> matchMovingUp = SimulateMovement(fruit, Direction.Up);
            List<Fruit> matchMovingDown = SimulateMovement(fruit, Direction.Down);
            List<Fruit> matchMovingLeft = SimulateMovement(fruit, Direction.Left);
            List<Fruit> matchMovingRight = SimulateMovement(fruit, Direction.Right);

            List<Fruit> bestMatch = matchMovingUp;

            if (matchMovingDown.Count > bestMatch.Count) bestMatch = matchMovingDown;
            if (matchMovingLeft.Count > bestMatch.Count) bestMatch = matchMovingLeft;
            if (matchMovingRight.Count > bestMatch.Count) bestMatch = matchMovingRight;

            return bestMatch;

        }

        private List<Fruit> SimulateMovement(Fruit fruit, Direction direction)
        {
            Fruit[,] tempBoardFruit = new Fruit[_boardGrid.Columns, _boardGrid.Rows];
            Array.Copy(_boardGrid.BoardFruitArray, tempBoardFruit, _boardGrid.BoardFruitArray.Length);

            List<Fruit> matches = new();

            int Column = fruit.Column;
            int Row = fruit.Row;

            Vector2Int movementDirection = MovementDirection.GetDirectionCoordinates(direction);
            Vector2Int selectedFruitPosition = new(Column, Row);
            Vector2Int swappedFruitPosition = new(Column + movementDirection.x, Row + movementDirection.y);

            if (!_boardGrid.HasTileAt(swappedFruitPosition.x, swappedFruitPosition.y)) return matches;

            Fruit selectedFruit = tempBoardFruit[selectedFruitPosition.x, selectedFruitPosition.y];
            Fruit swappedFruit = tempBoardFruit[swappedFruitPosition.x, swappedFruitPosition.y];

            tempBoardFruit[swappedFruitPosition.x, swappedFruitPosition.y] = selectedFruit;
            tempBoardFruit[selectedFruitPosition.x, selectedFruitPosition.y] = swappedFruit;

            List<Fruit> matchesFound = new();
            for (int i = 0; i < _boardGrid.Columns; i++)
            {
                for (int j = 0; j < _boardGrid.Rows; j++)
                {
                    // matchesFound.AddRange(GetFruitMatch(i, j, 1, 0, tempBoardFruit));
                    // matchesFound.AddRange(GetFruitMatch(i, j, 0, 1, tempBoardFruit));
                }
            }

            foreach (Fruit fruitFound in matchesFound)
            {
                if (!matches.Contains(fruitFound))
                {
                    matches.Add(fruitFound);
                }
            }
            return matches;
        }
    }
}
