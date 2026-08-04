using System.Linq;
using CAM.Contracts.RepositoryContracts.Entity;
using CAM.Entities;
using CAM.Entities.Models;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.Entity
{
    public class MajorSoftwareBuildRepository : RepositoryBase<Majorsoftwarebuilds>, IMajorSoftwareBuildRepository
    {
        ModelContext _context;
        public MajorSoftwareBuildRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }

        //public async Task<IEnumerable<MajorSoftwareBuild>> GetAllWithRelations()
        //{
        //    return await FindAll().OrderBy(m => m.OriginalEquipmentManufacturerId)
        //        .Include(v => v.VulnerabilityStatus)
        //        .Include(o => o.OriginalEquipmentManufacturer)
        //        .ToListAsync();
        //}

        public IQueryable<Majorsoftwarebuilds> GetAllWithRelations()
        {
            return FindAll();
        }

        public void Detach()
        {

            var plannedActivityType = typeof(Majorsoftwarebuilds);
            var changedEntriesCopy = _context.ChangeTracker.Entries()
                .Where(e => e.Entity.GetType() == plannedActivityType)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;


        }
    }
}
