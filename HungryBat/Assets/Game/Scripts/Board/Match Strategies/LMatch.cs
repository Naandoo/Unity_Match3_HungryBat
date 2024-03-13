namespace Board
{
    namespace MatchesStrategy
    {
        public class LMatch : MatchStrategy
        {
            public override int MinAmountVerticalItems => 3;

            public override int MaxAmountVerticalItems => 5;

            public override int MinAmountHorizontalItems => 3;

            public override int MaxAmountHorizontalItems => 5;
        }
    }
}