namespace ImageContestSystem.Web.Areas.Admin.Models.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using ImageContestSystem.Common.Mapping;
    using ImageContestSystem.Models;

    public class UserInputModel : IMapFrom<User>
    {
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "{0} must be in range {1}...{2}")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "{0} must be in range {1}...{2}")]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public int AccessFailedCount { get; set; }
        
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "{0} must be in range {1}...{2}")]
        public string FullName { get; set; }
    }
}