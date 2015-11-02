namespace ImageContestSystem.Web.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using ImageContestSystem.Data.UnitOfWork;
    using ImageContestSystem.Models;
    using ImageContestSystem.Web.Areas.Admin.Models.InputModels;
    using ImageContestSystem.Web.Areas.Admin.Models.ViewModels;

    using PagedList;

    public class UsersController : AdminBaseController
    {
        private const int DefaultUserPerPage = 50;

        private const int DefaultPage = 1;

        public UsersController(IImageContestSystemData data)
            : base(data)
        {
        }

        public UsersController(IImageContestSystemData data, User userProfile)
            : base(data, userProfile)
        {
        }

        // GET: Admin/Users
        public ActionResult Index(string username, int page = DefaultPage)
        {
            var users = this.ContestData.Users
                .All();

            if (!string.IsNullOrWhiteSpace(username))
            {
                users = users
                    .Where(u => u.UserName.ToLower().Contains(username.ToLower()));
            }

            var filtredUsers = users
                .OrderBy(u => u.UserName)
                .Project()
                .To<UserViewModel>();

            return this.Request.IsAjaxRequest()
                ? (ActionResult)this.PartialView("UserList", filtredUsers.ToPagedList(page, DefaultUserPerPage))
                : this.View(filtredUsers.ToPagedList(page, DefaultUserPerPage));
        }

        // GET: Admin/Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = this.ContestData.Users.Find(id);
            if (user == null)
            {
                return this.HttpNotFound();
            }

            var userView = Mapper.Map<User, UserViewModel>(user);

            return this.View(userView);
        }

        // GET: Admin/Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = this.ContestData.Users.Find(id);
            if (user == null)
            {
                return this.HttpNotFound();
            }

            var userView = Mapper.Map<User, UserInputModel>(user);

            return this.View(userView);
        }

        // POST: Admin/Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserInputModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = this.ContestData.Users
                    .All()
                    .FirstOrDefault(u => u.UserName == model.UserName);

                if (user == null)
                {
                    return this.HttpNotFound();
                }

                user.UserName = model.UserName;
                user.FullName = model.FullName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                
                this.ContestData.SaveChanges();

                return this.RedirectToAction("Index");
            }

            return this.View(model);
        }
    }
}