namespace ImageContestSystem.Models
{
    public class Vote
    {
        public int VoteId { get; set; }

        public string ParticipantId { get; set; }
        public virtual User Participant { get; set; }

        public int PictureId { get; set; }
        public virtual Picture Picture { get; set; }
    }
}
