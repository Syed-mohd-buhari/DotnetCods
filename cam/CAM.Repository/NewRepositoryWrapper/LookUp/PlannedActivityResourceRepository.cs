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
    public class PlannedActivityResourceRepository : RepositoryBaseNew<Plannedactivityresources>, IPlannedActivityResourceRepository
    {
        ModelContextNew _context;

        public PlannedActivityResourceRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;

        }


        public async Task<IEnumerable<Plannedactivityresources>> GetAllWithRelations()
        {
            throw new System.NotImplementedException();
        }

        public void Detach()
        {
            var data = typeof(Plannedactivityresources);
            var changedEntriesCopy = _context.ChangeTracker.Entries()
                .Where(e => e.Entity.GetType() == data)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }
    }
}