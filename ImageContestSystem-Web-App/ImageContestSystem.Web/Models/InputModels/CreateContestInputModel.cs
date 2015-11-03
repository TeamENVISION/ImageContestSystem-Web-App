using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageContestSystem.Common.Mapping;
using ImageContestSystem.Models;
using ImageContestSystem.Web.CustomAttributes;

namespace ImageContestSystem.Web.Models.InputModels
{
    public class CreateContestInputModel : IMapFrom<Contest>
    {
        public string Id { get; set; }
        [StringLength(30, ErrorMessage = "{0} must be between {2} and {1} characters long",
            MinimumLength = 6)]
        public string Title { get; set; }

        [StringLength(1000, ErrorMessage = "{0} must be between {2} and {1} characters long",
            MinimumLength = 10)]
        public string Description { get; set; }
        public DateTime StartDate { get; set; }

        //still not working
        [DateGreaterThan("EndDate", "StartDate", CompareType.GreaterThan)]
        public DateTime EndDate { get; set; }

        [Range(1, 20, ErrorMessage = "{0} must be in range between {1} and {2}")]
        public int VotesCount { get; set; }

        [Range(1, 30, ErrorMessage = "{0} must be in range between {1} and {2}")]
        public int WinnersCount { get; set; }
        public Contest Contests { get; set; }
        public List<string> SelectedParticipants { get; set; }
        public List<User> Users { get; set; }
        public List<string> SelectedVoters { get; set; }

        public IEnumerable<ParticipationType> ParticipationStrategy { get; set; }
        public List<string> SelectParticipationStrategy { get; set; }

        public IEnumerable<DeadlineType> DeadlineStrategy { get; set; }
        public List<string> SelectDeadlineStrategy { get; set; }

        public IEnumerable<VotingType> VotingStrategy { get; set; }
        public List<string> SelectVotingStrategy { get; set; }
        public CreateContestInputModel()
        {

        }

        public CreateContestInputModel(Contest _contest, List<User> _Users,
            IEnumerable<ParticipationType> participationStrategy,
            IEnumerable<DeadlineType> deadlineStrategy,
            IEnumerable<VotingType> votingStrategy)
        {
            this.Contests = _contest;
            this.Users = _Users;
            this.ParticipationStrategy = participationStrategy;
            this.DeadlineStrategy = deadlineStrategy;
            this.VotingStrategy = votingStrategy;

            SelectedParticipants = new List<string>();
            SelectedVoters = new List<string>();
            SelectParticipationStrategy = new List<string>();
            SelectDeadlineStrategy = new List<string>();
            SelectVotingStrategy = new List<string>();
        }


    }
}