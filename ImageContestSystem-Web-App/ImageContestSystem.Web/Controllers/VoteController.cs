using System.Web;
using ImageContestSystem.Web.Models.InputModels;

namespace ImageContestSystem.Web.Controllers
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
            string userId = this.UserProfile.Id;

            var contests = this.ContestData.Contest
                .All()
                .Where(c =>
                    (c.Voters.Any(v => v.Id == userId) || c.VotingType == VotingType.Open) &&
                    c.HasEnded == false)
                .Select(x => new VoteViewModel
                {
                    ContestId = x.ContestId,
                    Title = x.Title,
                    Description = x.Description,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    AvailableVotes = x.VotesCount - x.Pictures
                    .SelectMany(pi => pi.Votes)
                    .Count(p => p.VoterId == userId),
                    VotesCount = x.VotesCount,
                    HasEnded = x.HasEnded,
                    OwnerUsername = x.Owner.UserName,
                    PictureUrl = x.Pictures.FirstOrDefault().Url
                }).ToList();

            return View(contests);
        }

        // GET: All pictures from a single contest that you can vote to
        public ActionResult Contest(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            string userId = this.UserProfile.Id;

            var contest = this.ContestData.Contest.Find(id);
            if (contest == null)
            {
                return RedirectToAction("Index");
            }

            bool canVote = contest.VotingType == VotingType.Open || contest.Voters.Any(v => v.Id == userId);

            if (!canVote)
            {
                return RedirectToAction("Index");
            }

            var pictures = contest.Pictures.Select(p => new PictureViewModel
            {
                VotesCount = p.Votes.Count,
                HasVoted = p.Votes.Any(v=>v.VoterId == userId),
                UploaderUsername = p.Uploader.UserName,
                PictureId = p.PictureId,
                Url = p.Url
            }).ToList();

            return View(pictures);
        }

        [HttpPost]
        public ActionResult Vote(int pictureId)
        {
            string userId = this.UserProfile.Id;
            var contest = this.ContestData.Contest.All().FirstOrDefault(c => c.Pictures.Any(p => p.PictureId == pictureId));

            if (contest == null)
            {
                return RedirectToAction("Index");
            }

            var picture = contest.Pictures.AsQueryable().First(p=>p.PictureId == pictureId);

            bool canVote = contest.VotingType == VotingType.Open ||
                           contest.Voters.Any(v => v.Id == userId);

            if (!canVote)
            {
                return RedirectToAction("Index");
            }

            var vote = picture.Votes.FirstOrDefault(p => p.VoterId == userId && p.PictureId == pictureId);

            LikeButtonInputModel buttonResponse = new LikeButtonInputModel
            {
                HasVoted = true,
                VoteCount = picture.Votes.Count
            };

            if (vote == null)
            {
                int availableVotes = contest.VotesCount - contest.Pictures
                    .SelectMany(pi=>pi.Votes).Count(p => p.VoterId == userId);

                if (availableVotes >= 1)
                {
                    Vote newVote = new Vote
                    {
                        VoterId = userId,
                        PictureId = pictureId
                    };
                    this.ContestData.Votes.Add(newVote);
                    buttonResponse.VoteCount += 1;
                }
                else
                {
                    throw new HttpException(400, "You don't have enough available votes!");
                }
            }
            else
            {
                this.ContestData.Votes.Delete(vote);
                buttonResponse.HasVoted = false;
                buttonResponse.VoteCount -= 1;
            }
            this.ContestData.SaveChanges();
            return PartialView("_VoteButton", buttonResponse);
        }
    }

}