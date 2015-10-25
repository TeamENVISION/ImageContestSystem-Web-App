namespace ImageContestSystem.Web.Models.ViewModels
{
    using ImageContestSystem.Common.Mapping;
    using ImageContestSystem.Models;

    public class PrizeViewModel : IMapFrom<Prize>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string WinnerUserName { get; set; }
    }
}