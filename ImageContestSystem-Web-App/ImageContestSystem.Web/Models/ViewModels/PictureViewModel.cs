namespace ImageContestSystem.Web.Models.ViewModels
{
    using ImageContestSystem.Common.Mapping;
    using ImageContestSystem.Models;

    public class PictureViewModel : IMapFrom<Picture>
    {
        public string Url { get; set; }
    }
}