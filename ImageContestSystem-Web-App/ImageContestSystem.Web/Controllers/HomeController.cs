namespace ImageContestSystem.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using ImageContestSystem.Data.UnitOfWork;
    using ImageContestSystem.Models;
    using ImageContestSystem.Web.Models.ViewModels;

    using PagedList;

    public class HomeController : BaseController
    {
        private const int DefaultPageSize = 10;

        private const int DefaultPage = 1;

        public HomeController(IImageContestSystemData data)
            : base(data)
        {
        }

        public HomeController()
            : base(new ImageContestSystemData())
        {
        }

        public ActionResult Index(int page = DefaultPage)
        {
            var contests = this.ContestData.Contest
                .All()
                .Where(c => !c.HasEnded)
                .OrderByDescending(c => c.EndDate)
                .Project()
                .To<ContestViewModel>();
            
            return this.View(contests.ToPagedList(page, DefaultPageSize));
        }

        public ActionResult About()
        {
            this.ViewBag.Message = "ICS description page and team introduction.";

            return this.View();
        }
    }
}