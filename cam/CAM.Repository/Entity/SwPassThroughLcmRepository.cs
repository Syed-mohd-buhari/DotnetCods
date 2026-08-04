using CAM.Contracts.RepositoryContracts.Entity;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Repository.Entity
{
    public class SwPassThroughLcmRepository : RepositoryBase<Swpassthroughlcm>, ISwPassThroughRepositoryLcm
    {
        ModelContext _modelContext;
        public SwPassThroughLcmRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _modelContext = repositoryContext;
        }

        public void Detach()
        {

            var passthrough = typeof(Swpassthroughlcm);
            var changedEntriesCopy = _modelContext.ChangeTracker.Entries()
                .Where(e => e.Entity.GetType() == passthrough)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;


        }
    }
}
