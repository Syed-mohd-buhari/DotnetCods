using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models.Lookup;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class ActivityStatusRepository : RepositoryBaseNew<Activitystatuses>, IActivityStatusRepository
    {
        public ActivityStatusRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

        public IQueryable<Activitystatuses> GetAll()
        {

            var data = FindAll().OrderBy(x => x.Activitystatus).Include(p => p.CreationuserNavigation)
                .Include(p => p.ModificationuserNavigation).Include(x => x.Plannedactivities).ToList();

            return FindAll().OrderBy(x => x.Activitystatus).Include(p => p.CreationuserNavigation)
                .Include(p => p.ModificationuserNavigation).Include(x => x.Plannedactivities)
                .AsQueryable();
        }

        public IQueryable<Activitystatuses> GetAllWithRelations()
        {
            return  FindAll().OrderBy(x => x.Activitystatus).Include(p => p.CreationuserNavigation)
                .Include(p => p.ModificationuserNavigation)
                .Include(x => x.Plannedactivities).ThenInclude(x => x.Activitystatus).Include(x => x.Plannedactivities)
                .ThenInclude(x => x.Activitystatus).Include(x => x.Plannedactivities).ThenInclude(x => x.Activitystatus)
                .AsQueryable();
        }

        
    }
}