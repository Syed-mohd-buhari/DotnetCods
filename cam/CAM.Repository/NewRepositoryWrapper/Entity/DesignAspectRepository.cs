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
    public class DesignAspectRepository : RepositoryBaseNew<Designaspects>, IDesignAspectRepository
    {
        ModelContextNew _context;
        public DesignAspectRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
        public async Task<IEnumerable<Designaspects>> GetAllWithRelations()
        {
            return await FindAll()
                .ToListAsync();
        }
        public void Detach()
        {

            var plannedActivityType = typeof(Designaspects);
            var changedEntriesCopy = _context.ChangeTracker.Entries()
                .Where(e => e.Entity.GetType() == plannedActivityType)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;


        }
    }
}
