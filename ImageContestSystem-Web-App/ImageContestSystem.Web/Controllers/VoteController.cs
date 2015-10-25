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
                        //.Where(p=> p.ContestId == m.ContestId)
                    .SelectMany(v => v.Votes)
                    .Count(vo => vo.ParticipantId == userId && vo.Picture.ContestId == m.ContestId)));


            var contests = this.ContestData.Contest.All()
                .Where(c => (c.Voters.Any(v => v.Id == userId) || c.ContestStrategy.VotingStrategy == false) && c.HasEnded == false)
                .Project()
                .To<VoteViewModel>();

            return View(contests);
        }

        //GET: All pictures from a single contest that you can vote to
        public ActionResult Contest(int id)
        {
            string userId = this.User.Identity.GetUserId();

            var contest = this.ContestData.Contest.Find(id);
            if (contest == null)
            {
                return HttpNotFound();
            }

            bool canVote = contest.ContestStrategy.VotingStrategy == false || contest.Voters.Any(v => v.Id == userId);

            if (!canVote)
            {
                return HttpNotFound();
            }

            var conf = Mapper.Configuration;
            conf.CreateMap<Picture, PictureViewModel>();

            var pictures = contest.Pictures.AsQueryable().Project().To<PictureViewModel>();
            return View(pictures);
        }


        public ActionResult Vote(int id)
        {
            string userId = this.User.Identity.GetUserId();
            var picture = this.ContestData.Pictures.Find(id);

            if (picture == null)
            {
                return HttpNotFound();
            }

            bool canVote = picture.Contest.ContestStrategy.VotingStrategy == false ||
                           picture.Contest.Participants.Any(p => p.Id == userId);

            if (!canVote)
            {
                return HttpNotFound();
            }

            var vote = picture.Votes.FirstOrDefault(p => p.ParticipantId == userId && p.PictureId == id);

            if (vote == null)
            {
                int availableVotes = picture.Contest.VotesCount - picture.Votes.Count(p => p.ParticipantId == userId);
                if (availableVotes >= 1)
                {
                    Vote newVote = new Vote();
                    newVote.ParticipantId = userId;
                    newVote.PictureId = id;
                    this.ContestData.Votes.Add(newVote);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                this.ContestData.Votes.Delete(vote);
            }
            this.ContestData.SaveChanges();
            return HttpNotFound();
        }
    }

}