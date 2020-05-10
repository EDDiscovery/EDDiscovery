namespace EliteDangerous.HistoryList
{
    public struct FsdJumpStatistics
    {
        public int Count;
        public double Distance;
        public int BasicBoosts;
        public int StandardBoosts;
        public int PremiumBoosts;

        public FsdJumpStatistics(int count, double distance, int basicBoosts, int standardBoosts, int premiumBoosts)
        {
            Count = count;
            Distance = distance;
            BasicBoosts = basicBoosts;
            StandardBoosts = standardBoosts;
            PremiumBoosts = premiumBoosts;
        }
    }
}
