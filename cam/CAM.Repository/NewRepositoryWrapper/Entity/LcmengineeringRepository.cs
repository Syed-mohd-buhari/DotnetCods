using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.Entity;
using CAM.Entities;
using CAM.Entities.Models;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.Entity
{
    public class LcmengineeringRepository : RepositoryBaseNew<Lcmengineering>, ILcmEngineeringRepository
    {
        ModelContextNew _context;

        public LcmengineeringRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }

        public async Task<IEnumerable<Lcmengineering>> GetAllWithRelations()
        {
            return await FindAll()
                .ToListAsync();
        }
        public void Detach()
        {

            var plannedActivityType = typeof(Lcmengineering);
            var changedEntriesCopy = _context.ChangeTracker.Entries()
                .Where(e => e.Entity.GetType() == plannedActivityType)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;


        }

    }
}
