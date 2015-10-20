using ImageContestSystem.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ImageContestSystem.Data
{
    public class ImageContestSystemContext : IdentityDbContext<User>, IImageContestSystemContext
    {
        public ImageContestSystemContext()
            : base("ImageContestSystem", throwIfV1Schema: false)
        {
        }

        public static ImageContestSystemContext Create()
        {
            return new ImageContestSystemContext();
        }
    }
}
