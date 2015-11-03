using System.Collections;
using System.Data.Entity;
using System.Net;
using System.Web.UI.WebControls.WebParts;
using Glimpse.Core.Extensions;
using Ninject.Infrastructure.Language;
using WebGrease.Css.Extensions;

namespace ImageContestSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;

    using ImageContestSystem.Common.DropBox;
    using ImageContestSystem.Data.UnitOfWork;
    using ImageContestSystem.Models;
    using ImageContestSystem.Web.Models.InputModels;
    using ImageContestSystem.Web.Models.ViewModels;

    using Microsoft.AspNet.Identity;

    [Authorize]
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
            var ownerId = this.User.Identity.GetUserId();
            var contests = this.ContestData.Contest.All().Where(c=>c.OwnerId == ownerId)
                .Include(c => c.Owner);
            return View(contests.ToList());
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

            var participationStrategy = Enum.GetValues(typeof(ParticipationType))
                                                       .Cast<ParticipationType>();
            var deadlineStrategy = Enum.GetValues(typeof(DeadlineType))
                                                       .Cast<DeadlineType>();
            var votingnStrategy = Enum.GetValues(typeof(VotingType))
                                                       .Cast<VotingType>();

            CreateContestInputModel inputModel = new CreateContestInputModel(contest, users, participationStrategy, deadlineStrategy, votingnStrategy);

            return this.View(inputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateContestInputModel model)
        {
            var ownerId = this.User.Identity.GetUserId();

            var getParticipants = new List<User>();
            var getVoters = new List<User>();

            var participationStrategy = Enum.GetValues(typeof(ParticipationType))
                                                   .Cast<ParticipationType>();
            var deadlineStrategy = Enum.GetValues(typeof(DeadlineType))
                                                       .Cast<DeadlineType>();
            var votingnStrategy = Enum.GetValues(typeof(VotingType))
                                                       .Cast<VotingType>();
            var users = this.ContestData.Users.All().ToList();

            if (model != null && this.ModelState.IsValid)
            {
                var contest = new Contest()
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    VotesCount = model.VotesCount,
                    WinnersCount = model.WinnersCount,
                    OwnerId = ownerId,
                    HasEnded = false,
                    ContestStatus = ContestStatus.Active,                 
                    ParticipationType = (ParticipationType)Enum.Parse(typeof(ParticipationType), model.SelectParticipationStrategy.FirstOrDefault()),
                    VotingType = (VotingType)Enum.Parse(typeof(VotingType), model.SelectVotingStrategy.FirstOrDefault()),
                    DeadlineType = (DeadlineType)Enum.Parse(typeof(DeadlineType), model.SelectDeadlineStrategy.FirstOrDefault())
                };
               
                this.ContestData.Contest.Add(contest);
                contest.Pictures.Add(new Picture { Url = "default.jpg", UploaderId = ownerId });
                this.ContestData.SaveChanges();

                contest.Participants = GetParticipants(contest.ContestId,model, getParticipants);
                contest.Voters = GetVoters(contest.ContestId,model, getVoters);
                this.ContestData.SaveChanges();

                return this.RedirectToAction("Index");
            }

            model.ParticipationStrategy = participationStrategy;
            model.DeadlineStrategy = deadlineStrategy;
            model.VotingStrategy = votingnStrategy;
            model.Users = users;

            return this.View(model);
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            var contests = new Contest();
            if (id == null)
            {
                return this.RedirectToAction("Index");
            }
            var contest = this.ContestData.Contest.Find(id);
            var a = contest.VotingType;

            if (contest == null)
            {
                return  this.RedirectToAction("Index");
            }
           
            var users = this.ContestData.Users.All().ToList();
            
            var model = new UpdateContestInputModel(contests, users);
            model.VoteType = a;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int id,UpdateContestInputModel model)
        {
            var getVoters = new List<User>();
            if (id == null)
            {
                return this.RedirectToAction("Index");
            }
            var contest = this.ContestData.Contest.Find(id);

            if (contest == null)
            {
                return this.RedirectToAction("Index");
            }

            if (model.SelectedVoters != null)
            {
                foreach (var voterId in model.SelectedVoters)
                {
                    var user = this.ContestData.Users.All().First(u => u.Id == voterId);
                    getVoters.Add(user);
                }
            }

            contest.EndDate = model.EndDate;
            contest.Description = model.Description;
            contest.Voters = getVoters;

            this.ContestData.Contest.Update(contest);
            this.ContestData.SaveChanges();

            return this.RedirectToAction("Index");
        }


        public ActionResult Dismiss(int id)
        {
            var contest = this.ContestData.Contest.Find(id);

            contest.HasEnded = true;

            this.ContestData.Contest.Update(contest);
            this.ContestData.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Finalize(int id)
        {
            var getWinners = new List<string>();
            var contest = this.ContestData.Contest.Find(id);

            var duplicates = this.ContestData.Votes.All()
                .Where(c=>c.Picture.ContestId == contest.ContestId)
                .GroupBy(i => i.PictureId)
                     .Where(x => x.Count() > 0)
                     .OrderByDescending(x=>x.Count())
                     .Select(val => val.Key).ToList();

            foreach (var elem in duplicates)
            {
                var uploaderUsername = this.ContestData.Pictures.All()
                    .Where(p => p.PictureId == elem)
                    .Select(p => p.Uploader.UserName).Single();
                getWinners.Add(uploaderUsername);
            }
            contest.HasEnded = true;

            var winners = getWinners.Distinct().Take(contest.WinnersCount);

            return View(winners.ToList());
        }

        public List<User> GetParticipants(int contestId,CreateContestInputModel model, List<User> participants )
        {
            var ownerId = this.User.Identity.GetUserId();
            var userName = this.Profile.UserName;
            var contestName = model.Title;
                
            if (model.SelectedParticipants != null)
            {
                foreach (var id in model.SelectedParticipants)
                {
                    var user = this.ContestData.Users.All()
                        .First(u => u.Id == id);

                    var participantNotification = new Notification()
                    {
                        RecipientId = id,
                        SenderId = ownerId,
                        Content =
                            string.Format("{0} has invited you to be a participant in {1} contest", userName,
                                contestName),
                        CreatedOn = DateTime.Now,
                        IsRead = false,
                        InviteType = InvitationType.Participant,
                        ContestId = contestId
                    };
                    this.ContestData.Notifications.Add(participantNotification);
                    
                    participants.Add(user);
                }
            }
            this.ContestData.SaveChanges();

            return participants;
        }

        public List<User> GetVoters(int contestId, CreateContestInputModel model, List<User> voters)
        {
            var ownerId = this.User.Identity.GetUserId();
            var userName = this.Profile.UserName;
            var contestName = model.Title;

            if (model.SelectedVoters != null)
            {
                foreach (var id in model.SelectedVoters)
                {
                    var user = this.ContestData.Users.All().First(u => u.Id == id);
                    var voterNotification = new Notification()
                    {
                        RecipientId = id,
                        SenderId = ownerId,
                        Content =
                            string.Format("{0} has invited you to be a voter in {1} contest", userName,
                                contestName),
                        CreatedOn = DateTime.Now,
                        IsRead = false,
                        InviteType = InvitationType.Voter,
                        ContestId = contestId
                    };
                    this.ContestData.Notifications.Add(voterNotification);
                    voters.Add(user);
                }
            }
            this.ContestData.SaveChanges();
            return voters;
        } 
    }
}
