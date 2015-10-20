namespace ImageContestSystem.Models
{
    public class ContestStrategy
    {
        public int ContestStrategyId { get; set; }
        public int WinnerCount { get; set; }
        public bool VotingStrategy { get; set; }
        public bool ParticipatingStrategy { get; set; }
        public bool DeadlineStrategy { get; set; }
    }
}
