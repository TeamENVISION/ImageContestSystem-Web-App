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
        public ICollection<User> Participants { get; set; }
    }
}