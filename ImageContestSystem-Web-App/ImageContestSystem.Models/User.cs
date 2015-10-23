namespace ImageContestSystem.Models
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class User : IdentityUser
    {
        private ICollection<Contest> contestParticipants;

        private ICollection<Contest> contestVoters;

        private ICollection<Picture> pictures;

        public User()
        {
            this.contestParticipants = new HashSet<Contest>();
            this.contestVoters = new HashSet<Contest>();
            this.pictures = new HashSet<Picture>();
        }

        public string FullName { get; set; }

        public virtual ICollection<Contest> ContestParticipants
        {
            get
            {
                return this.contestParticipants;
            }

            set
            {
                this.contestParticipants = value;
            }
        }

        public virtual ICollection<Contest> ContestVoters
        {
            get
            {
                return this.contestVoters;
            }

            set
            {
                this.contestVoters = value;
            }
        }

        public virtual ICollection<Picture> Pictures
        {
            get
            {
                return this.pictures;
            }

            set
            {
                this.pictures = value;
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here
            return userIdentity;
        }
    }
}