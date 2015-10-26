namespace ImageContestSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using ImageContestSystem.Models;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

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
                ContestStatus = ContestStatus.Dismissed,
                DeadlineType = DeadlineType.PracticantsLimit,
                VotingType = VotingType.Close,
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
                ContestStatus = ContestStatus.Active,
                DeadlineType = DeadlineType.EndDate,
                VotingType = VotingType.Open,
                VotesCount = 20
            };

            context.Contests.Add(contest);
            context.Contests.Add(contest2);

            if (!context.Roles.Any(r => r.Name == "admin"))
            {
                CreateAdminRole(context);
            }

            if (!context.Users.Any(u => u.UserName == "admin"))
            {
                var store = new UserStore<User>(context);
                var userManager = new UserManager<User>(store);
                var admin = new User
                {
                    UserName = "admin",
                    Email = "admin@admin.com"
                };

                userManager.Create(admin, "@dminPass1");

                userManager.AddToRole(admin.Id, "admin");
            }

            context.SaveChanges();
        }
        
        private static void CreateAdminRole(ImageContestSystemContext context)
        {
            var store = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(store);

            roleManager.Create(new IdentityRole("admin"));
        }
    }
}