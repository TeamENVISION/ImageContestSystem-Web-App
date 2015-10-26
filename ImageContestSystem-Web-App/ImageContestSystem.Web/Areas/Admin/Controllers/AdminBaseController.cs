namespace ImageContestSystem.Web.Areas.Admin.Controllers
{
    using System.Web.Mvc;

    using ImageContestSystem.Data.UnitOfWork;
    using ImageContestSystem.Models;
    using ImageContestSystem.Web.Controllers;

    [Authorize(Roles = "admin")]
    public abstract class AdminBaseController : BaseController
    {
        protected AdminBaseController(IImageContestSystemData data)
            : base(data)
        {
        }

        protected AdminBaseController(IImageContestSystemData data, User userProfile)
            : base(data, userProfile)
        {
        }
    }
}