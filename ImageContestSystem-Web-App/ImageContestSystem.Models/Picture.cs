namespace ImageContestSystem.Models
{
    public class Picture
    {
        public int PictureId { get; set; }

        public string ParticipantId { get; set; }
        public virtual User Participant { get; set; }

        public int ContestId { get; set; }
        public virtual Contest Contest { get; set; }

        public string Url { get; set; }
    }
}
