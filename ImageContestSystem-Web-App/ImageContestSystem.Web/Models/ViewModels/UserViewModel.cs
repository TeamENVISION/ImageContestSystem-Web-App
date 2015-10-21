namespace ImageContestSystem.Web.Models.ViewModels
{
    using ImageContestSystem.Common.Mapping;
    using ImageContestSystem.Models;

    public class UserViewModel : IMapFrom<User>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }
    }
}