using System.Collections.Generic;
using FruitItem;
using UnityEngine;

namespace Board
{
    //TO DO: Consider taking a look at Gil tips and change system based on it
    //continue from creating the tip visually and system to handle 
    public class BoardAuthenticator : MonoBehaviour
    {
        [SerializeField] private BoardGrid _boardGrid;
        [SerializeField] private BoardSorter _boardSorter;


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                print(" Has match " + ContainsAvailableMatches(_boardGrid.BoardFruitArray));

            }
        }

        public bool ContainsAvailableMatches(Fruit[,] boardFruits)
        {
            Match match = new();

            for (int i = 0; i < boardFruits.GetLength(0); i++)
            {
                for (int j = 0; j < boardFruits.GetLength(1); j++)
                {
                    if (!_boardGrid.HasTileAt(i, j)) continue;

                    Fruit fruit = _boardGrid.BoardFruitArray[i, j];

                    Match matchPossibility = FindBestMatchFromSimulatedPossibilities(fruit, _boardGrid.BoardFruitArray);

                    if (matchPossibility.Count >= match.Count)
                    {
                        match = matchPossibility;
                    }
                }
            }

            return match.Count >= 3;
        }


        private Match FindBestMatchFromSimulatedPossibilities(Fruit fruit, Fruit[,] boardFruit)
        {
            Match matchMovingRight = SimulateMovement(fruit, Direction.Right, boardFruit);
            Match matchMovingUp = SimulateMovement(fruit, Direction.Up, boardFruit);
            Match matchMovingDown = SimulateMovement(fruit, Direction.Down, boardFruit);
            Match matchMovingLeft = SimulateMovement(fruit, Direction.Left, boardFruit);

            Match bestMatch = matchMovingUp;

            if (matchMovingDown.Count > bestMatch.Count) bestMatch = matchMovingDown;
            if (matchMovingLeft.Count > bestMatch.Count) bestMatch = matchMovingLeft;
            if (matchMovingRight.Count > bestMatch.Count) bestMatch = matchMovingRight;

            return bestMatch;

        }

        private Match SimulateMovement(Fruit fruit, Direction direction, Fruit[,] tempBoardFruit)
        {
            Match matches = new();

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


            for (int i = 0; i < _boardGrid.Columns; i++)
            {
                for (int j = 0; j < _boardGrid.Rows; j++)
                {
                    matches.Clone(GetFruitMatch(i, j, tempBoardFruit));
                }
            }

            tempBoardFruit[selectedFruitPosition.x, selectedFruitPosition.y] = selectedFruit;
            tempBoardFruit[swappedFruitPosition.x, swappedFruitPosition.y] = swappedFruit;
            return matches;
        }

        public Match GetFruitMatch(int startColumn, int startRow, Fruit[,] boardFruit)
        {
            Match currentSequence = new();
            Match bestMatch = new();

            FruitType? currentFruitType = null;

            for (int i = startColumn; i >= 0 && i < boardFruit.GetLength(0); i += 1)
            {
                for (int j = startRow; j >= 0 && j < boardFruit.GetLength(1); j += 1)
                {

                    if (!_boardGrid.HasTileAt(i, j)) continue;

                    Fruit fruit = boardFruit[i, j];

                    if (fruit == null)
                    {
                        currentSequence.Clear();
                        currentFruitType = null;
                        continue;
                    }

                    if (!currentFruitType.HasValue || currentFruitType == fruit.FruitID.FruitType)
                    {
                        currentSequence.Add(fruit.Column, fruit.Row);
                        currentFruitType = fruit.FruitID.FruitType;

                        if (currentSequence.Count >= 3)
                        {
                            if (currentSequence.Count > bestMatch.Count)
                            {
                                bestMatch.Clear();
                                bestMatch.Clone(currentSequence);
                            }
                        }
                    }
                    else
                    {
                        currentSequence.Clear();
                        currentSequence.Add(fruit.Column, fruit.Row);
                        currentFruitType = fruit.FruitID.FruitType;
                    }
                }
            }

            return bestMatch;
        }
    }

    public struct Match
    {
        public int LeftCount { get; private set; }
        public int RightCount { get; private set; }
        public int AboveCount { get; private set; }
        public int BelowCount { get; private set; }
        public Vector2Int StartingPosition { get; private set; }

        public readonly int Count { get => LeftCount + RightCount + AboveCount + BelowCount; }

        public void Add(int column, int row)
        {
            if (Count == 0)
            {
                StartingPosition = new Vector2Int(column, row);
            }
            else
            {
                RightCount += (column > StartingPosition.x) ? 1 : 0;
                LeftCount += (column < StartingPosition.x) ? 1 : 0;
                AboveCount += (row > StartingPosition.y) ? 1 : 0;
                BelowCount += (row < StartingPosition.y) ? 1 : 0;
            }
        }

        public void Clear()
        {
            LeftCount = 0;
            RightCount = 0;
            AboveCount = 0;
            BelowCount = 0;
        }

        public void Clone(Match match)
        {
            this.LeftCount = match.LeftCount;
            this.RightCount = match.RightCount;
            this.AboveCount = match.AboveCount;
            this.BelowCount = match.BelowCount;
        }
    }
}
