using System.ComponentModel.DataAnnotations;

namespace ImageContestSystem.Models
{
    public class Vote
    {
        [Key]
        public int VoteId { get; set; }
        [Required]
        public string ParticipantId { get; set; }
        public virtual User Participant { get; set; }
        [Required]
        public int PictureId { get; set; }
        public virtual Picture Picture { get; set; }
    }
}
