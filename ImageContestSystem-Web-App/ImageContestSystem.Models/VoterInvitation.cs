namespace ImageContestSystem.Models
{
    public class VoterInvitation : Invitation
    {
        public int ContestVotersId { get; set; }
        public Contest Type { get; set; }
    }
}
