using CAM.Contracts.RepositoryContracts.Entity;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Repository.Entity
{
    public class TemsFntReportRepository : RepositoryBase<Temsfntreport>, ITemsFntReportRepository
    {
        ModelContext _modelContext;
        public TemsFntReportRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _modelContext = repositoryContext;
        }

        public void Detach()
        {

            var temsFntReport = typeof(Temsfntreport);
            var changedEntriesCopy = _modelContext.ChangeTracker.Entries()
                .Where(e => e.Entity.GetType() == temsFntReport)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;


        }
    }
}
