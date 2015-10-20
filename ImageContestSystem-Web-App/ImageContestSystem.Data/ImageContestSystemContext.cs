using System.Data.Entity;
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

        public IDbSet<Contest> Contests { get; set; }
        public IDbSet<ContestStrategy> ContestStrategies { get; set; }
        public IDbSet<Picture> Pictures { get; set; }
        public IDbSet<Vote> Votes { get; set; }
    }
}
