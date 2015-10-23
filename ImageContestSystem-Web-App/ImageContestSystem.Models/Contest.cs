namespace ImageContestSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Contest
    {
        private ICollection<User> participants;

        private ICollection<Picture> pictures;

        private ICollection<User> voters;

        public Contest()
        {
            this.participants = new HashSet<User>();
            this.voters = new HashSet<User>();
            this.pictures = new HashSet<Picture>();
        }

        [Key]
        public int ContestId { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(30)]
        public string Title { get; set; }

        [Required]
        [MinLength(30)]
        [MaxLength(1000)]
        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int VotesCount { get; set; }

        public bool HasEnded { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }

        public int ContestStrategyId { get; set; }

        public virtual ContestStrategy ContestStrategy { get; set; }

        public virtual ICollection<User> Participants
        {
            get
            {
                return this.participants;
            }

            set
            {
                this.participants = value;
            }
        }

        public virtual ICollection<User> Voters
        {
            get
            {
                return this.voters;
            }

            set
            {
                this.voters = value;
            }
        }

        public virtual ICollection<Picture> Pictures
        {
            get
            {
                return this.pictures;
            }

            set
            {
                this.pictures = value;
            }
        }
    }
}