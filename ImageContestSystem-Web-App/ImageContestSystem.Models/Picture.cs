using System.Collections.Generic;

namespace ImageContestSystem.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Picture
    {
        private ICollection<Vote> votes;

        public Picture()
        {
            this.votes = new HashSet<Vote>();
        }

        [Key]
        public int PictureId { get; set; }

        [Required]
        public string UploaderId { get; set; }

        public virtual User Uploader { get; set; }

        [Required]
        public int ContestId { get; set; }

        public virtual Contest Contest { get; set; }

        [Required]
        public string Url { get; set; }

        public virtual ICollection<Vote> Votes
        {
            get { return this.votes; }
            set { this.votes = value; }
        }
    }
}