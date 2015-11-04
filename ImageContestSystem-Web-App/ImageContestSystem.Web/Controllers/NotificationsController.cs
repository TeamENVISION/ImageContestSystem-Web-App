using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using ImageContestSystem.Data.UnitOfWork;
using ImageContestSystem.Models;
using ImageContestSystem.Web.Models.InputModels;
using ImageContestSystem.Web.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace ImageContestSystem.Web.Controllers
{
    public class NotificationsController : BaseController
    {
        public NotificationsController(IImageContestSystemData data) : base(data)
        {
        }

        public NotificationsController(IImageContestSystemData data, User userProfile) : base(data, userProfile)
        {
        }


        // GET: Notifications
        public ActionResult Index()
        {
            string userId = this.User.Identity.GetUserId();
            var notifications = this.ContestData.Notifications.All()
                .Where(n=>n.RecipientId == userId)
                .Select(n=> new NotificationViewModel
                {
                    NotificationId = n.Id,
                    Sender = n.Sender.UserName,
                    Content = n.Content,
                    InviteType = n.InviteType,
                    CreatedOn = n.CreatedOn
                }).ToList();
            return View(notifications);
        }


        public ActionResult Reply(int? id)
        {

            string userId = this.User.Identity.GetUserId();
            var notification = this.ContestData.Notifications.Find(id);
            bool canReply = notification.RecipientId == userId && notification.InviteType != InvitationType.Custom;
            if (id == null || !canReply)
            {
                return RedirectToAction("Index");
            }


            notification.ReadOn = DateTime.Now;

            NotificationViewModel notificationView = new NotificationViewModel
            {
                NotificationId = notification.Id,
                Sender = notification.Sender.UserName,
                Content = notification.Content,
                InviteType = notification.InviteType,
                CreatedOn = notification.CreatedOn
            };
            this.ContestData.SaveChanges();

            return View(notificationView);
        }

        [HttpPost]
        public ActionResult UserResponse(int notificationId, bool accepted)
        {
            var userId = this.User.Identity.GetUserId();
            var user = this.ContestData.Users.Find(userId);
            var notification = this.ContestData.Notifications.All().FirstOrDefault(n => n.RecipientId == userId && n.Id == notificationId);
        
            if (notification == null || notification.InviteType == InvitationType.Custom)
            {
                return RedirectToAction("Index");
            }

            Notification newNotification = new Notification
            {
                SenderId = userId,
                RecipientId = notification.SenderId,
                Content = String.Format("{0} has declined your {1} invitation.", user.UserName, notification.InviteType),
                CreatedOn = DateTime.Now,
                InviteType = InvitationType.Custom,
                ContestId = notification.ContestId
            };

            if (accepted)
            {
                newNotification.Content = String.Format("{0} has accepted your invitation.", user.UserName);
                if (notification.InviteType == InvitationType.Participant)
                {
                    this.ContestData.Contest.Find(notification.ContestId).Participants.Add(user);
                }
                if (notification.InviteType == InvitationType.Voter)
                {
                    this.ContestData.Contest.Find(notification.ContestId).Voters.Add(user);
                }
            }
            notification.InviteType = InvitationType.Custom;
            this.ContestData.Notifications.Add(newNotification);
            this.ContestData.SaveChanges();
            return View("Index");
        }


    }
}