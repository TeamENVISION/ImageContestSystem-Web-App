namespace ImageContestSystem.Web.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using ImageContestSystem.Data.UnitOfWork;
    using ImageContestSystem.Models;
    using ImageContestSystem.Web.Models.ViewModels;

    using PagedList;

    public class HomeController : AdminBaseController
    {
        private const int DefaultPageSize = 10;

        private const int DefaultPage = 1;

        public HomeController(IImageContestSystemData data)
            : base(data)
        {
        }

        public HomeController(IImageContestSystemData data, User userProfile)
            : base(data, userProfile)
        {
        }

        // GET: Admin/Home
        public ActionResult Index(int page = DefaultPage)
        {
            var contests = this.ContestData.Contest
                .All()
                .OrderByDescending(c => c.EndDate)
                .Project()
                .To<ContestViewModel>();

            return this.View(contests.ToPagedList(page, DefaultPageSize));
        }
    }
}