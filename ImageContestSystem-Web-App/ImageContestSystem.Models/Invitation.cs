namespace ImageContestSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Invitation
    {
        [Required]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        public int ContestId { get; set; }

        public virtual Contest Contest { get; set; }

        public Guid UniqueIdentifier { get; set; }

        public bool IsUsed { get; set; }

        public bool IsVoter { get; set; }

        public bool IsPracticant { get; set; }

        [Required]
        public string InviterId { get; set; }

        public virtual User Inviter { get; set; }
    }
}