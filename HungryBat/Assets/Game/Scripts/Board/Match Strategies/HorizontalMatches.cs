namespace Board
{
    namespace MatchesStrategy
    {
        public class HorizontalMatch : MatchStrategy
        {
            public override int MinAmountVerticalItems => 0;

            public override int MaxAmountVerticalItems => 0;

            public override int MinAmountHorizontalItems => 3;

            public override int MaxAmountHorizontalItems => 5;
        }
    }
}
