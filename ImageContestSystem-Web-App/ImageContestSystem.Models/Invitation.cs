namespace ImageContestSystem.Models
{
    using System;

    public abstract class Invitation
    {
        public string UserId { get; set; }

        public virtual User User { get; set; }

        public int ContestId { get; set; }

        public virtual Contest Contest { get; set; }

        public Guid UniqueIdentifier { get; set; }

        public string InviterId { get; set; }

        public virtual User Inviter { get; set; }
    }
}