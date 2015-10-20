using System.Web.Mvc;
using System.Web.Mvc.Expressions;
using ImageContestSystem.Data.UnitOfWork;

namespace ImageContestSystem.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IImageContestSystemData data)
            : base(data)
        {
        }

        public HomeController()
            : base(new ImageContestSystemData())
        {

        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}