namespace ImageContestSystem.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using ImageContestSystem.Data.UnitOfWork;
    using ImageContestSystem.Models;
    using ImageContestSystem.Web.Models.ViewModels;

    using Microsoft.AspNet.Identity;

    public class UsersController : BaseController
    {
        public UsersController(IImageContestSystemData data)
            : base(data)
        {
        }

        public UsersController(IImageContestSystemData data, User userProfile)
            : base(data, userProfile)
        {
        }

        public ActionResult Index()
        {
            var userId = this.User.Identity.GetUserId();
            var user = this.ContestData.Users
                .All()
                .Where(x => x.Id == userId)
                .Project()
                .To<UserViewModel>()
                .FirstOrDefault();

            return this.View(user);
        }
    }
}