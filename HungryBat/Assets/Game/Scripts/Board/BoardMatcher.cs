using UnityEngine;
using BoardItem;
using Board.MatchesStrategy;
using DG.Tweening;
using System.Collections.Generic;

namespace Board
{
    public class BoardMatcher : MonoBehaviour
    {
        [SerializeField] private BoardGrid _boardGrid;
        private MatchStrategies matchStrategies = new();
        private List<Fruit> EqualFruits = new();

        public void TryMatchFruit(int Column, int Row, Direction direction)
        {
            EqualFruitsCount equalFruitsCount = GetEqualFruitsCountStartingAt(Column, Row);
            MatchStrategy matchStrategy = matchStrategies.GetMostValuableMatch(equalFruitsCount);
            Fruit fruit = _boardGrid.BoardFruitArray[Column, Row];

            if (matchStrategy == null)
            {
                AnimateItemWrongAttempt(fruit, direction);
            }
            else
            {
                ExecuteMatch(fruit, equalFruitsCount, matchStrategy);
            }
        }

        private EqualFruitsCount GetEqualFruitsCountStartingAt(int Column, int Row)
        {
            EqualFruitsCount equalFruitsCount = new();
            FruitType fruitType = _boardGrid.BoardFruitArray[Column, Row].FruitID.FruitType;

            equalFruitsCount.horizontal += GetFruitsCountIn(Column, Row, fruitType, columnDirection: -1, rowDirection: 0);
            equalFruitsCount.horizontal += GetFruitsCountIn(Column, Row, fruitType, columnDirection: 1, rowDirection: 0);
            equalFruitsCount.vertical += GetFruitsCountIn(Column, Row, fruitType, columnDirection: 0, rowDirection: -1);
            equalFruitsCount.vertical += GetFruitsCountIn(Column, Row, fruitType, columnDirection: 0, rowDirection: 1);

            return equalFruitsCount;
        }

        private int GetFruitsCountIn(int startColumn, int startRow,
        FruitType selectedFruitType, int columnDirection, int rowDirection)
        {
            int amountOfFruits = 0;

            int currentColumn = startColumn + columnDirection;
            int currentRow = startRow + rowDirection;

            while (IsValidPosition(currentColumn, currentRow) &&
            _boardGrid.BoardFruitArray[currentColumn, currentRow].FruitID.FruitType == selectedFruitType)
            {
                amountOfFruits++;
                currentColumn += columnDirection;
                currentColumn += rowDirection;

                Fruit fruit = _boardGrid.BoardFruitArray[currentColumn, currentRow];
                EqualFruits.Add(fruit);
            }

            return amountOfFruits;
        }

        private bool IsValidPosition(int column, int row)
        {
            return column >= 0 && column < _boardGrid.Columns && row >= 0 && row < _boardGrid.Rows;
        }

        private void AnimateItemWrongAttempt(Fruit fruit, Direction direction)
        {
            Vector2Int Coordinates = MovementDirection.GetDirectionCoordinates(direction);

            int currentColumn = fruit.Column;
            int currentRow = fruit.Row;

            int trialColumn = currentColumn + Coordinates.x;
            int trialRow = currentRow + Coordinates.y;
            Fruit trialFruit = _boardGrid.BoardFruitArray[trialColumn, trialRow];

            Vector3 fruitPosition = _boardGrid.GetFruitPosition(currentColumn, currentRow);
            Vector3 trialPosition = _boardGrid.GetFruitPosition(trialColumn, trialRow);

            float duration = 0.25f;

            Sequence sequenceAnimation = DOTween.Sequence();
            sequenceAnimation.Append(fruit.transform.DOMove(trialPosition, duration));
            sequenceAnimation.Join(trialFruit.transform.DOMove(fruitPosition, duration));
            sequenceAnimation.Append(fruit.transform.DOMove(fruitPosition, duration));
            sequenceAnimation.Join(trialFruit.transform.DOMove(trialPosition, duration));
        }

        private void ExecuteMatch(Fruit initialMatchFruit, EqualFruitsCount equalFruitsCount, MatchStrategy matchStrategy)
        {
            int totalVerticalFruits = equalFruitsCount.vertical;
            int totalHorizontalFruits = equalFruitsCount.horizontal;

            int verticalMatches = matchStrategy.GetSumVerticalMatches(totalVerticalFruits);
            int horizontalMatches = matchStrategy.GetSumHorizontalMatches(totalHorizontalFruits);

            List<Fruit> MatchFruits = new() { initialMatchFruit };

            for (int i = 0; i < verticalMatches; i++)
            {
                if (EqualFruits[i].Column == initialMatchFruit.Column)
                {
                    verticalMatches--;
                    MatchFruits.Add(EqualFruits[i]);
                }
            }

            for (int i = 0; i < horizontalMatches; i++)
            {
                if (EqualFruits[i].Row == initialMatchFruit.Row)
                {
                    horizontalMatches--;
                    MatchFruits.Add(EqualFruits[i]);
                }
            }

            foreach (Fruit fruit in MatchFruits)
            {
                fruit.Vanish();
            }
        }
    }

    public class EqualFruitsCount
    {
        public int horizontal = 1;
        public int vertical = 1;
    }
}