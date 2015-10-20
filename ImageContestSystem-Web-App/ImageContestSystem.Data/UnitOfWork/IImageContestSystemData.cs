using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageContestSystem.Data.Repository;
using ImageContestSystem.Models;

namespace ImageContestSystem.Data.UnitOfWork
{
    public interface IImageContestSystemData
    {
        IRepository<User> Users { get; }
        IRepository<Contest> Contest { get; }
        IRepository<ContestStrategy> ContestStrategies { get; }
        IRepository<Picture> Pictures { get; }
        IRepository<Vote> Votes { get; }

        int SaveChanges();
    }
}
