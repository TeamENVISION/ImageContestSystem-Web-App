using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageContestSystem.Web.Models.InputModels
{
    public class LikeButtonInputModel
    {
        public bool HasVoted { get; set; }
        public int VoteCount { get; set; }
    }
}