using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ImageContestSystem.Data;
using ImageContestSystem.Data.UnitOfWork;
using ImageContestSystem.Models;
using ImageContestSystem.Web.Models.InputModels;
using Microsoft.AspNet.Identity;

namespace ImageContestSystem.Web.Controllers
{
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
            return View();
        }

        public ActionResult Create()
        {
            var contest = new Contest();
            var users = this.ContestData.Users.All().ToList();
            CreateContestInputModel inputModel = new CreateContestInputModel(contest,users);
            return View(inputModel);
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
                    var user = this.ContestData.Users.All().Where(u => u.Id == id).First();
                    getParticipants.Add(user);
                    
                }
            }
            if (model.SelectedVoters != null)
            {
                foreach (var id in model.SelectedVoters)
                {
                    var user = this.ContestData.Users.All().Where(u => u.Id == id).First();
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
                    ContestStrategyId = 1,
                    Participants = getParticipants,
                    Voters = getVoters
                   
                };
              
                this.ContestData.Contest.Add(contest);
                this.ContestData.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

    }
}
