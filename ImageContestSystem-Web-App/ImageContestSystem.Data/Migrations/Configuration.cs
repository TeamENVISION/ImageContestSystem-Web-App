namespace ImageContestSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

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
            var contest = new Contest
                              {
                                  Owner = context.Users.FirstOrDefault(u => u.UserName == "bucanero"),
                                  StartDate = DateTime.Now,
                                  EndDate = DateTime.Now.AddDays(20),
                                  HasEnded = false,
                                  Title = "Iceland landscape contest",
                                  Description = "Some cool description for this contest. Description.",
                                  ContestStrategy = new ContestStrategy
                                    {
                                        DeadlineStrategy = true,
                                        WinnerCount = 2
                                    },
                                    VotesCount = 10
                              };
            var contest2 = new Contest
            {
                Owner = context.Users.FirstOrDefault(u => u.UserName == "bucanero"),
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(20),
                HasEnded = false,
                Title = "Iceland landscape contest2",
                Description = "Some cool description for this contest. Description.2",
                ContestStrategy = new ContestStrategy
                {
                    DeadlineStrategy = true,
                    WinnerCount = 1
                },
                VotesCount = 20
            };

            context.Contests.Add(contest);
            context.Contests.Add(contest2);
            
            context.SaveChanges();
        }
    }
}