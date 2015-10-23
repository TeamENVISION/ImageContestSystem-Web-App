namespace ImageContestSystem.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Prize
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(200)]
        public string Description { get; set; }
        
        public int ContestId { get; set; }

        public virtual Contest Contest { get; set; }
    }
}