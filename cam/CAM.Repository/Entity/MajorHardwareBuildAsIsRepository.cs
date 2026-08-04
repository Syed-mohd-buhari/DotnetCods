using CAM.Contracts.RepositoryContracts.Entity;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Repository.Entity
{
    public class MajorHardwareBuildAsIsRepository : RepositoryBase<Majorhardwarebuildasis>, IMajorHardwareBuildAsIsRepository
    {
        ModelContext _context;

        public MajorHardwareBuildAsIsRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        } 
        public IQueryable<Majorhardwarebuildasis> GetAllWithRelations()
        {
            return FindAll();
        }
        public void Detach()
        {

            var majorhardwarebuildsasis = typeof(Majorhardwarebuildasis);
            var changedEntriesCopy = _context.ChangeTracker.Entries()
                .Where(e => e.Entity.GetType() == majorhardwarebuildsasis)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;


        }

    }
}
