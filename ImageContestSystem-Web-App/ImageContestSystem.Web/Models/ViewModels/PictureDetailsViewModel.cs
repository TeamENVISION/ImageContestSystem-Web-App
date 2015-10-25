namespace ImageContestSystem.Web.Models.ViewModels
{
    using AutoMapper;

    using ImageContestSystem.Common.Mapping;
    using ImageContestSystem.Models;

    public class PictureDetailsViewModel : IMapFrom<Picture>, IHaveCustomMappings
    {
        public int PictureId { get; set; }

        public string Author { get; set; }
        
        public string Url { get; set; }

        public int VotesCount { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Picture, PictureDetailsViewModel>()
                .ForMember(c => c.Author, cd => cd.MapFrom(m => m.Uploader.UserName))
                .ForMember(c => c.Url, cd => cd.MapFrom(m => m.Url))
                .ForMember(c => c.VotesCount, cd => cd.MapFrom(m => m.Votes.Count));
        }
    }
}