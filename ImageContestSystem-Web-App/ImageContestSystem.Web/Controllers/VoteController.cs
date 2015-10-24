using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ImageContestSystem.Data.UnitOfWork;
using ImageContestSystem.Models;
using ImageContestSystem.Web.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace ImageContestSystem.Web.Controllers
{
    [Authorize]
    public class VoteController : BaseController
    {
        public VoteController(IImageContestSystemData data) : base(data)
        {
        }

        public VoteController(IImageContestSystemData data, User userProfile) : base(data, userProfile)
        {
        }
        // GET: Available Contests For Voting
        public ActionResult Index()
        {
            var conf = Mapper.Configuration;
            conf.CreateMap<VoteViewModel, VoteViewModel>();
            string userId = this.User.Identity.GetUserId();

            var contests = this.ContestData.Contest.All()
                .Where(c => (c.Voters.Any(v=>v.Id == userId) || c.ContestStrategy.VotingStrategy == false) && c.HasEnded == false)
                .Select(c => new VoteViewModel
                {
                    ContestId = c.ContestId,
                    Title = c.Title,
                    Description = c.Description,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    AvailableVotes = c.VotesCount - c.Participants.FirstOrDefault(p => p.Id == userId).Votes.Count(v=>v.Picture.ContestId == c.ContestId),
                    VotesCount = c.VotesCount,
                    HasEnded = c.HasEnded,
                    OwnerUsername = c.Owner.UserName
                })
                .Project()
                .To<VoteViewModel>();

            return View(contests);
        }


    }
}