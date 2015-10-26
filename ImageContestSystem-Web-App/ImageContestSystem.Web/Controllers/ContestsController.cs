using System.Collections;
using System.Web.UI.WebControls.WebParts;
using Glimpse.Core.Extensions;
using Ninject.Infrastructure.Language;

namespace ImageContestSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;

    using ImageContestSystem.Data.UnitOfWork;
    using ImageContestSystem.Models;
    using ImageContestSystem.Web.Models.InputModels;
    using ImageContestSystem.Web.Models.ViewModels;

    using Microsoft.AspNet.Identity;

    public class ContestsController : BaseController
    {
        public ContestsController(IImageContestSystemData data)
            : base(data)
        {
        }

        public ContestsController(IImageContestSystemData data, User userProfile)
            : base(data, userProfile)
        {
        }

        // GET: Contests
        public ActionResult Index()
        {
            return this.View();
        }

        // GET: Contests/{contestId}
        public ActionResult DetailsById(int id)
        {
            var contest = Mapper
                .Map<DetailsContestViewModel>(this.ContestData.Contest.Find(id));

            if (contest == null)
            {
                return this.HttpNotFound();
            }

            var daysLeft = contest.EndDate - DateTime.Now;
            contest.DaysLeft = daysLeft.Days;

            return this.View(contest);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var contest = new Contest();
            var users = this.ContestData.Users.All().ToList();

            IEnumerable<ParticipationType> participationStrategy = Enum.GetValues(typeof(ParticipationType))
                                                       .Cast<ParticipationType>();

            CreateContestInputModel inputModel = new CreateContestInputModel(contest, users, participationStrategy);

            return this.View(inputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateContestInputModel model)
        {
            var ownerId = this.User.Identity.GetUserId();

            var getParticipants = new List<User>();
            var getVoters = new List<User>();


            if (model.SelectedParticipants != null)
            {
                foreach (var id in model.SelectedParticipants)
                {
                    var user = this.ContestData.Users.All().First(u => u.Id == id);
                    getParticipants.Add(user);
                }
            }

            if (model.SelectedVoters != null)
            {
                foreach (var id in model.SelectedVoters)
                {
                    var user = this.ContestData.Users.All().First(u => u.Id == id);
                    getVoters.Add(user);
                }
            }

            if (model != null && this.ModelState.IsValid)
            {
                var contest = new Contest()
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    VotesCount = model.VotesCount,
                    OwnerId = ownerId,
                    HasEnded = false,
                    ContestStatus = ContestStatus.Active,
                    Participants = getParticipants,
                    Voters = getVoters,
                    ParticipationType = (ParticipationType)Enum.Parse(typeof(ParticipationType), model.SelectParticipationStrategy.FirstOrDefault())

                };

                this.ContestData.Contest.Add(contest);
                this.ContestData.SaveChanges();

                return this.RedirectToAction("Index");
            }

            return this.View(model);
        }


    }
}
