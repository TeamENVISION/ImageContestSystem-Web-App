namespace ImageContestSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string RecipientId { get; set; }

        public virtual User Recipient { get; set; }
        
        [Required]
        public string SenderId { get; set; }

        public virtual User Sender { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(150)]
        public string Content { get; set; }
        
        public DateTime CreatedOn { get; set; }

        public DateTime? ReadOn { get; set; }
        
        [Required]
        public bool IsRead { get; set; }
        [Required]
        public InvitationType InviteType { get; set; }

        [Required]
        public int ContestId { get; set; }
    }
}