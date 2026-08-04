using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models.Lookup;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
   public partial class ActivityDetailsRepository : RepositoryBaseNew<Activitydetails>, IActivityDetailsRepository
    {
       public ActivityDetailsRepository(ModelContextNew repositoryContext) : base(repositoryContext)
       {
       }
       public IQueryable<Activitydetails> GetAllWithRelations()
       {
            return FindAll()
                .Include(p => p.CreationuserNavigation)
                .Include(p => p.ModificationuserNavigation);
  
       }
    }
}
