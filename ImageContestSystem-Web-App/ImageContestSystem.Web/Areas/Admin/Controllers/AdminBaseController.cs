using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageContestSystem.Web.Areas.Admin.Controllers
{
    using ImageContestSystem.Data.UnitOfWork;
    using ImageContestSystem.Models;
    using ImageContestSystem.Web.Controllers;

    []
    public class AdminBaseController : BaseController
    {
        public AdminBaseController(IImageContestSystemData data)
            : base(data)
        {
        }

        public AdminBaseController(IImageContestSystemData data, User userProfile)
            : base(data, userProfile)
        {
        }
    }
}