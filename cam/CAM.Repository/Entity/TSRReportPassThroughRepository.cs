using CAM.Contracts.RepositoryContracts.Entity;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Repository.Entity
{
    public class TSRReportPassThroughRepository : RepositoryBase<Tsrpassthrough>, ITsrPassThroughRepository
    {
        ModelContext _modelContext;
        public TSRReportPassThroughRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _modelContext = repositoryContext;
        }

        public void Detach()
        {

            var tsrpassthrough = typeof(Tsrpassthrough);
            var changedEntriesCopy = _modelContext.ChangeTracker.Entries()
                .Where(e => e.Entity.GetType() == tsrpassthrough)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;


        }
    }
}
