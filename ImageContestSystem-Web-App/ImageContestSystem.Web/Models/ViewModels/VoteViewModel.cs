﻿namespace ImageContestSystem.Web.Models.ViewModels
{
    using System;
    using System.Collections.Generic;

    using ImageContestSystem.Common.Mapping;
    using ImageContestSystem.Models;

    public class VoteViewModel : IMapFrom<Contest>
    {
        public int ContestId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int AvailableVotes { get; set; }

        public int VotesCount { get; set; }

        public bool HasEnded { get; set; }

        public string OwnerUsername { get; set; }

        public string PictureUrl { get; set; }

    }
}