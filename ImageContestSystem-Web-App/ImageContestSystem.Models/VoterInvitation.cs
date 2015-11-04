using System.ComponentModel.DataAnnotations;

namespace ImageContestSystem.Models
{
    public class VoterInvitation : Invitation
    {
        [Required]
        public int ContestVotersId { get; set; }
    }
}
