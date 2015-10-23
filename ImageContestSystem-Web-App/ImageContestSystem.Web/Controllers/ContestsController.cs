using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ImageContestSystem.Data;
using ImageContestSystem.Models;
using ImageContestSystem.Web.Models.InputModels;
using Microsoft.AspNet.Identity;

namespace ImageContestSystem.Web.Controllers
{
    public class ContestsController : Controller
    {
        private ImageContestSystemContext db = new ImageContestSystemContext();

        // GET: Contests
        public ActionResult Index()
        {
            var contests = db.Contests.Include(c => c.ContestStrategy).Include(c => c.Owner);
            return View(contests.ToList());
        }

        public ActionResult Create()
        {    
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateContestInputModel model)
        {
            var ownerId = this.User.Identity.GetUserId();

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

                };

                this.db.Contests.Add(contest);
                this.db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

       
        // POST: Contests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contest contest = db.Contests.Find(id);
            db.Contests.Remove(contest);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
