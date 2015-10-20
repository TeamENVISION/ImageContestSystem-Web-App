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

        // GET: Contests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contest contest = db.Contests.Find(id);
            if (contest == null)
            {
                return HttpNotFound();
            }
            return View(contest);
        }

        // GET: Contests/Create
        public ActionResult Create()
        {
            ViewBag.ContestStrategyId = new SelectList(db.ContestStrategies, "ContestStrategyId", "ContestStrategyId");
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Contests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContestId,Title,Description,StartDate,EndDate,HasEnded,OwnerId,ContestStrategyId")] Contest contest)
        {
            if (ModelState.IsValid)
            {
                db.Contests.Add(contest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ContestStrategyId = new SelectList(db.ContestStrategies, "ContestStrategyId", "ContestStrategyId", contest.ContestStrategyId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "Email", contest.OwnerId);
            return View(contest);
        }

        // GET: Contests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contest contest = db.Contests.Find(id);
            if (contest == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContestStrategyId = new SelectList(db.ContestStrategies, "ContestStrategyId", "ContestStrategyId", contest.ContestStrategyId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "Email", contest.OwnerId);
            return View(contest);
        }

        // POST: Contests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContestId,Title,Description,StartDate,EndDate,HasEnded,OwnerId,ContestStrategyId")] Contest contest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ContestStrategyId = new SelectList(db.ContestStrategies, "ContestStrategyId", "ContestStrategyId", contest.ContestStrategyId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "Email", contest.OwnerId);
            return View(contest);
        }

        // GET: Contests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contest contest = db.Contests.Find(id);
            if (contest == null)
            {
                return HttpNotFound();
            }
            return View(contest);
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
