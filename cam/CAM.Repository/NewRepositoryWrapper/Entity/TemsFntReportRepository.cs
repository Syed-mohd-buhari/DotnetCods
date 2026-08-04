using System.Linq;
using CAM.Contracts.RepositoryContracts.Entity;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.Entity
{
    public class TemsFntReportRepository : RepositoryBaseNew<Temsfntreport>, ITemsFntReportRepository
    {
        ModelContextNew _repositoryContext;
        public TemsFntReportRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }
        public void Detach()
        {

            var tmsFntReport = typeof(Temsfntreport);
            var changedEntriesCopy = _repositoryContext.ChangeTracker.Entries()
                .Where(e => e.Entity.GetType() == tmsFntReport)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;


        }
    }
}
