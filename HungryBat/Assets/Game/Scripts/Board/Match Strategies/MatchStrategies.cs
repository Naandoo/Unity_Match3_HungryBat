using System.Collections.Generic;
namespace Board
{
    namespace MatchesStrategy
    {
        public class MatchStrategies
        {
            private List<MatchStrategy> matchStrategies = new()
            {
                new HorizontalMatch(),
                new VerticalMatch(),
                new CrossMatch(),
                new LMatch(),
            };

            public MatchStrategy GetMostValuableMatch(EqualFruitsCount equalFruitsCount)
            {
                int currentMatchSum = 0;
                MatchStrategy mostValuableMatch = null;
                int verticalItems = equalFruitsCount.vertical;
                int horizontalItems = equalFruitsCount.horizontal;

                foreach (MatchStrategy matchStrategy in matchStrategies)
                {
                    int tempMatchSum = matchStrategy.GetSumOfMatches(verticalItems, horizontalItems);
                    if (tempMatchSum > currentMatchSum)
                    {
                        currentMatchSum = tempMatchSum;
                        mostValuableMatch = matchStrategy;
                    }
                }

                return mostValuableMatch;
            }
        }
    }
}