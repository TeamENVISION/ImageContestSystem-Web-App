namespace ImageContestSystem.Data.UnitOfWork
{
    using ImageContestSystem.Data.Repository;
    using ImageContestSystem.Models;

    public interface IImageContestSystemData
    {
        IRepository<User> Users { get; }

        IRepository<Contest> Contest { get; }
        
        IRepository<Picture> Pictures { get; }

        IRepository<Vote> Votes { get; }

        IRepository<Notification> Notifications { get; }

        IRepository<Prize> Prizes { get; } 

        int SaveChanges();
    }
}