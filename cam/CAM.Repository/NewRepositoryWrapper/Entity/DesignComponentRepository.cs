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
    public class DesignComponentRepository : RepositoryBaseNew<Designcomponents>, IDesignComponentRepository
    {
        private ModelContextNew _context;
        public DesignComponentRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }

        public async Task<IEnumerable<Designcomponents>> GetAllWithRelations()
        {
            return await FindAll()
                .ToListAsync();
        }

        public void Detach()
        {

            var data = typeof(Designcomponents);
            var changedEntriesCopy = _context.ChangeTracker.Entries()
                .Where(e => e.Entity.GetType() == data)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;


        }
    }
}
