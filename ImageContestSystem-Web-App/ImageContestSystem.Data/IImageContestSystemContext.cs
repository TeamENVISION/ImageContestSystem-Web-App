using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageContestSystem.Data
{
    public interface IImageContestSystemContext
    {
        int SaveChanges();
    }
}
