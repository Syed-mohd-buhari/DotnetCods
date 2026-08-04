using CAM.Contracts.RepositoryContracts.Entity;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System.Linq;
namespace CAM.Repository.NewRepositoryWrapper.Entity
{
    public class MajorHardwareBuildAsIsRepository : RepositoryBaseNew<Majorhardwarebuildasis>, IMajorHardwareBuildAsIsRepository
    {
        ModelContextNew _context;

        public MajorHardwareBuildAsIsRepository(ModelContextNew repositoryContext) : base(repositoryContext)
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
