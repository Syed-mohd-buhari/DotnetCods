using CAM.Contracts.RepositoryContracts.Entity;
using CAM.Entities;
using CAM.Entities.Models;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Repository.NewRepositoryWrapper.Entity
{
    public class PlannedActivityRepository : RepositoryBaseNew<Plannedactivities>, IPlannedActivityRepository
    {
        ModelContextNew _context;
        public PlannedActivityRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }

        public void Detach()
        {

            var plannedActivityType = typeof(Plannedactivities);
            var changedEntriesCopy = _context.ChangeTracker.Entries()
                  .Where(e => e.Entity.GetType() == plannedActivityType)
                  .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;


        }
    }
}
