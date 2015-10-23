namespace ImageContestSystem.Data.Migrations
{
    using System.Data.Entity.Migrations;

    using ImageContestSystem.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ImageContestSystemContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
            this.ContextKey = "ImageContestSystem.Data.ImageContestSystemContext";
        }

        protected override void Seed(ImageContestSystemContext context)
        {
        }
    }
}