namespace ImageContestSystem.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Picture
    {
        [Key]
        public int PictureId { get; set; }

        [Required]
        public string ParticipantId { get; set; }

        public virtual User Participant { get; set; }

        [Required]
        public int ContestId { get; set; }

        public virtual Contest Contest { get; set; }

        [Required]
        public string Url { get; set; }
    }
}