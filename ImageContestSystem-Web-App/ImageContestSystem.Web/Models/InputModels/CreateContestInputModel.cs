using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageContestSystem.Common.Mapping;
using ImageContestSystem.Models;

namespace ImageContestSystem.Web.Models.InputModels
{
    public class CreateContestInputModel : IMapFrom<Contest>
    {
        public string Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int VotesCount { get; set; }
        public Contest Contests { get; set; }
        public List<string> SelectedParticipants { get; set; }
        public List<User> Users { get; set; }

        public List<string> SelectedVoters { get; set; }

        public CreateContestInputModel()
        {
            
        }

        public CreateContestInputModel(Contest _contest, List<User> _Users)
        {
            this.Contests = _contest;
            this.Users = _Users;
            SelectedParticipants = new List<string>();
            SelectedVoters = new List<string>();
        }

    }
}