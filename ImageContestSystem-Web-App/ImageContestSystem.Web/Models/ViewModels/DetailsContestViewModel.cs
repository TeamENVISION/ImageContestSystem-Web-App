namespace ImageContestSystem.Web.Models.ViewModels
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;

    using ImageContestSystem.Common.Mapping;
    using ImageContestSystem.Models;

    public class DetailsContestViewModel : IMapFrom<Contest>, IHaveCustomMappings
    {
        public int ContestId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int DaysLeft { get; set; }

        public string Owner { get; set; }

        public string OwnerId { get; set; }

        public ContestStatus Status { get; set; }

        public VotingType VotingType { get; set; }

        public ParticipationType ParticipationType { get; set; }

        public DeadlineType DeadlineType { get; set; }

        public int PicturesCount { get; set; }

        public int ParticipantsCount { get; set; }

        public int VotesCount { get; set; }

        public int WinnerCount { get; set; }

        public IEnumerable<PrizeViewModel> Prizes { get; set; }

        public IEnumerable<PictureDetailsViewModel> Pictures { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Contest, DetailsContestViewModel>()
                .ForMember(c => c.ParticipantsCount, cd => cd.MapFrom(m => m.Participants.Count))
                .ForMember(c => c.VotesCount, cd => cd.MapFrom(m => m.VotesCount));
        }
    }
}