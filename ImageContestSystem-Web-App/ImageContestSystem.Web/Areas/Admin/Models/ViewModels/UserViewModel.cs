namespace ImageContestSystem.Web.Areas.Admin.Models.ViewModels
{
    using ImageContestSystem.Common.Mapping;
    using ImageContestSystem.Models;

    public class UserViewModel : IMapFrom<User>
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public int AccessFailedCount { get; set; }

        public string FullName { get; set; }
    }
}