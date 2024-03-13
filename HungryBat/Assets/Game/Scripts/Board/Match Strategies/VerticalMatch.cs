namespace Board
{
    namespace MatchesStrategy
    {
        public class VerticalMatch : MatchStrategy
        {
            public override int MinAmountVerticalItems => 3;

            public override int MaxAmountVerticalItems => 5;

            public override int MinAmountHorizontalItems => 0;

            public override int MaxAmountHorizontalItems => 0;
        }
    }
}