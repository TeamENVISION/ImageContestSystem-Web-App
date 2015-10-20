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

        int SaveChanges();
    }
}
