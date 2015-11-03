namespace ImageContestSystem.Data
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using ImageContestSystem.Models;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ImageContestSystemContext : IdentityDbContext<User>, IImageContestSystemContext
    {
        public ImageContestSystemContext()
            : base("ImageContestSystem", false)
        {
        }

        public IDbSet<Contest> Contests { get; set; }

        public IDbSet<Picture> Pictures { get; set; }

        public IDbSet<Vote> Votes { get; set; }

        public IDbSet<Notification> Notifications { get; set; }

        public IDbSet<Prize> Prizes { get; set; }

        public static ImageContestSystemContext Create()
        {
            return new ImageContestSystemContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contest>()
                .HasMany(c => c.Participants)
                .WithMany(u => u.ContestParticipants)
                .Map(
                   m =>
                   {
                       m.MapLeftKey("ContestId");
                       m.MapRightKey("ParticipantId");
                       m.ToTable("ContestParticipants");
                   });

            modelBuilder.Entity<Contest>()
                .HasMany(c => c.Voters)
                .WithMany(u => u.ContestVoters)
                .Map(
                   m =>
                   {
                       m.MapLeftKey("ContestId");
                       m.MapRightKey("VoterId");
                       m.ToTable("ContestVoters");
                   });

            modelBuilder.Conventions
                .Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
