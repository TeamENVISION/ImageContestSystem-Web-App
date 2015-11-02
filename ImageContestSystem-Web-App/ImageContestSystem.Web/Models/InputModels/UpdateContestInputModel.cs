using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ImageContestSystem.Models;

namespace ImageContestSystem.Web.Models.InputModels
{
    public class UpdateContestInputModel
    {
        public string Id { get; set; }
        [StringLength(1000, ErrorMessage = "{0} must be between {2} and {1} characters long",
           MinimumLength = 10)]
        public string Description { get; set; }

        public DateTime EndDate { get; set; }
        public Contest Contests { get; set; }
        public List<User> Users { get; set; }
        public List<string> SelectedVoters { get; set; }
        public VotingType VoteType { get; set; }
         public UpdateContestInputModel()
        {

        }

         public UpdateContestInputModel(Contest _contest, List<User> _Users)
        {
            this.Contests = _contest;
            this.Users = _Users;

            SelectedVoters = new List<string>();
        }

    }
}