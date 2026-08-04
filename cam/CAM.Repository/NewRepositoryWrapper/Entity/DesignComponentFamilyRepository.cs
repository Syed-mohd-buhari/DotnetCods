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
    public class DesignComponentFamilyRepository : RepositoryBaseNew<Designcomponentfamilies>, IDesignComponentFamilyRepository
    {
        ModelContextNew _context;

        public DesignComponentFamilyRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;

        }

        public async Task<IEnumerable<Designcomponentfamilies>> GetAllWithRelations()
        {
            return await FindAll().ToListAsync();
        }

        public void Detach()
        {

            var data = typeof(Designcomponentfamilies);
            var changedEntriesCopy = _context.ChangeTracker.Entries()
                .Where(e => e.Entity.GetType() == data)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;


        }
    }
}
