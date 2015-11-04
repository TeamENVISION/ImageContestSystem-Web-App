using System;
using System.ComponentModel.DataAnnotations;
using ImageContestSystem.Models;

namespace ImageContestSystem.Web.Models.ViewModels
{
    public class NotificationViewModel
    {
        [Required]
        public int NotificationId { get; set; }
        [Required]
        public string Sender { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public InvitationType InviteType { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}