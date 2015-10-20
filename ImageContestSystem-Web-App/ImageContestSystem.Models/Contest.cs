using System;
using System.Collections.Generic;

namespace ImageContestSystem.Models
{
    public class Contest
    {
        private ICollection<User> participants;
        private ICollection<User> voters;


        public Contest()
        {
            this.participants = new HashSet<User>();
            this.voters = new HashSet<User>();
        }

        public int ContestId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool HasEnded { get; set; }


        public string OwnerId { get; set; }
        public virtual User Owner { get; set; }

        public int ContestStrategyId { get; set; }
        public virtual ContestStrategy ContestStrategy { get; set; }

        public virtual ICollection<User> Participants
        {
            get { return this.participants; }
            set { this.participants = value; }
        }

        public virtual ICollection<User> Voters
        {
            get { return this.voters; }
            set { this.voters = value; }
        }

    }
}
