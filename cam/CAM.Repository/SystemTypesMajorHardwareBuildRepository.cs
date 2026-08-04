using CAM.Contracts.RepositoryContracts;
using CAM.Entities;
using CAM.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.Cross;
using CAM.Entities.Models.Cross;
using OracleModels.DBModels;
using OracleModels.DBContext;
using System.Linq;

namespace CAM.Repository
{
    public class SystemTypesMajorHardwareBuildRepository : RepositoryBase<Systemtypesmajorhardwarebuilds>, ISystemTypesMajorHardwareBuildRepository
    {
        private ModelContext _context;
        public SystemTypesMajorHardwareBuildRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }

        public void Detach()
        {

            var data = typeof(SystemTypesMajorHardwareBuild);
            var changedEntriesCopy = _context.ChangeTracker.Entries()
                .Where(e => e.Entity.GetType() == data)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;


        }

        public async Task<IEnumerable<Systemtypesmajorhardwarebuilds>> GetAllWithRelations()
        {
            return await FindAll()
                .ToListAsync();
        }


    }
}
