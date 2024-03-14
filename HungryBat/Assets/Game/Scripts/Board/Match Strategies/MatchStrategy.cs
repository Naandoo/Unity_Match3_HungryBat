using System;

namespace Board
{
    namespace MatchesStrategy
    {
        public abstract class MatchStrategy
        {
            abstract public int MinAmountVerticalItems { get; }
            abstract public int MaxAmountVerticalItems { get; }
            abstract public int MinAmountHorizontalItems { get; }
            abstract public int MaxAmountHorizontalItems { get; }

            public int GetSumOfMatches(int verticalItems, int horizontalItems)
            {
                if (ValidateMatchStrategy(verticalItems, horizontalItems))
                {
                    int totalVerticalItems = GetSumVerticalMatches(verticalItems);
                    int totalHorizontalItems = GetSumHorizontalMatches(horizontalItems);

                    int SumOfMatches = totalVerticalItems + totalHorizontalItems;
                    return SumOfMatches;
                }
                else return 0;
            }

            public int GetSumVerticalMatches(int verticalItems)
            {
                return Math.Min(verticalItems, MaxAmountVerticalItems);
            }

            public int GetSumHorizontalMatches(int horizontalItems)
            {
                return Math.Min(horizontalItems, MaxAmountHorizontalItems);
            }

            private bool ValidateMatchStrategy(int verticalItems, int horizontalItems)
            {
                if (IsWithinRange(verticalItems, MinAmountVerticalItems, MaxAmountVerticalItems)
                && IsWithinRange(horizontalItems, MinAmountHorizontalItems, MaxAmountHorizontalItems))
                {
                    return true;
                }
                else return false;
            }

            private bool IsWithinRange(int amount, int minAmount, int maxAmount)
            {
                if (amount >= minAmount && amount <= maxAmount) return true;
                else return false;
            }

            //create method to get the bricks to merge?
        }
    }
}
