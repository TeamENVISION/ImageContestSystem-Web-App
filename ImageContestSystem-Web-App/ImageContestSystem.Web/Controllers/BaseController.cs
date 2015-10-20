﻿namespace ImageContestSystem.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using ImageContestSystem.Data.UnitOfWork;
    using ImageContestSystem.Models;

    public abstract class BaseController : Controller
    {
        private IImageContestSystemData data;
        private User userProfile;

        public BaseController(IImageContestSystemData data)
        {
            this.TwitterData = data;
        }

        public BaseController(IImageContestSystemData data, User userProfile)
            : this(data)
        {
            this.UserProfile = userProfile;
        }

        protected IImageContestSystemData TwitterData { get; private set; }
        protected User UserProfile { get; private set; }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var username = requestContext.HttpContext.User.Identity.Name;
                var user = this.TwitterData.Users.All().FirstOrDefault(x => x.UserName == username);
                this.UserProfile = user;
            }

            return base.BeginExecute(requestContext, callback, state);
        }
    }
}