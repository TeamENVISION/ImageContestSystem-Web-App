﻿namespace ImageContestSystem.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using ImageContestSystem.Data.UnitOfWork;
    using ImageContestSystem.Models;
    using ImageContestSystem.Web.Models.ViewModels;
    using Microsoft.AspNet.Identity;

    [Authorize]
    public class VoteController : BaseController
    {
        public VoteController(IImageContestSystemData data)
            : base(data)
        {
        }

        public VoteController(IImageContestSystemData data, User userProfile)
            : base(data, userProfile)
        {
        }

        // GET: Available contests for voting
        public ActionResult Index()
        {
            string userId = this.User.Identity.GetUserId();

            var conf = Mapper.Configuration;
            conf.CreateMap<Contest, VoteViewModel>()
                .ForMember(c => c.AvailableVotes, sr => sr
                    .MapFrom(m => m.VotesCount - m.Pictures
                    .SelectMany(pi => pi.Votes)
                    .Count(p => p.VoterId == userId)));


            var contests = this.ContestData.Contest.All()
                .Where(c => (c.Voters.Any(v => v.Id == userId) || c.VotingType == VotingType.Open) && c.HasEnded == false)
                .Project()
                .To<VoteViewModel>();

            return View(contests);
        }

        // GET: All pictures from a single contest that you can vote to
        public ActionResult Contest(int id)
        {
            string userId = this.User.Identity.GetUserId();

            var contest = this.ContestData.Contest.Find(id);
            if (contest == null)
            {
                return HttpNotFound("The contest doesn't exist!");
            }

            bool canVote = contest.VotingType == VotingType.Open || contest.Voters.Any(v => v.Id == userId);

            if (!canVote)
            {
                return HttpNotFound("You cannot vote for this contest!");
            }

            var conf = Mapper.Configuration;
            conf.CreateMap<Picture, PictureViewModel>()
                .ForMember(p=> p.VotesCount, sr=> sr.MapFrom(m=> m.Votes.Count))
                .ForMember(p=> p.hasVoted, sr=> sr.MapFrom(m=> m.Votes.Any(v=>v.VoterId==userId)));

            var pictures = contest.Pictures.AsQueryable().Project().To<PictureViewModel>();
            return View(pictures);
        }

        [HttpPost]
        public ActionResult Vote(int pictureId)
        {
            string userId = this.User.Identity.GetUserId();
            var contest = this.ContestData.Contest.All().FirstOrDefault(c => c.Pictures.Any(p => p.PictureId == pictureId));

            if (contest == null)
            {
                return HttpNotFound("This picture doesn't exist!");
            }

            var picture = contest.Pictures.AsQueryable().First(p=>p.PictureId == pictureId);

            bool canVote = contest.VotingType == VotingType.Open ||
                           contest.Voters.Any(v => v.Id == userId);

            if (!canVote)
            {
                return HttpNotFound("You cannot vote for this contest!");
            }

            var vote = picture.Votes.FirstOrDefault(p => p.VoterId == userId && p.PictureId == pictureId);

            if (vote == null)
            {
                int availableVotes = contest.VotesCount - contest.Pictures
                    .SelectMany(pi=>pi.Votes).Count(p => p.VoterId == userId);

                if (availableVotes >= 1)
                {
                    Vote newVote = new Vote();
                    newVote.VoterId = userId;
                    newVote.PictureId = pictureId;
                    this.ContestData.Votes.Add(newVote);
                }

                else
                {
                    return this.HttpNotFound("You've reached the maximum limit of voting!");
                }
            }
            else
            {
                this.ContestData.Votes.Delete(vote);
            }
            this.ContestData.SaveChanges();
            int pictureVoteCount = picture.Votes.Count;
            return this.Content(pictureVoteCount.ToString());
        }
    }

}