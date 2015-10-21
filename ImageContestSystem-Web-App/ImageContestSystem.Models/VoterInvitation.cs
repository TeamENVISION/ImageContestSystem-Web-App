namespace ImageContestSystem.Models
{
    using System.ComponentModel.DataAnnotations;

    public class VoterInvitation : Invitation
    {
        [Required]
        public int ContestVotersId { get; set; }
    }
}