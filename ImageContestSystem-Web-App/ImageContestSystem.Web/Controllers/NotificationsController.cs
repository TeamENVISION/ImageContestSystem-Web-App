using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageContestSystem.Data.UnitOfWork;
using ImageContestSystem.Models;

namespace ImageContestSystem.Web.Controllers
{
    public class NotificationsController : BaseController
    {
        // GET: Notifications
        public ActionResult Index()
        {
            return View();
        }

        public NotificationsController(IImageContestSystemData data) : base(data)
        {
        }

        public NotificationsController(IImageContestSystemData data, User userProfile) : base(data, userProfile)
        {
        }
    }
}