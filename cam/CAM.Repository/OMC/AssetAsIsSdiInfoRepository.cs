using CAM.Contracts.RepositoryContracts.OMC;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Repository.OMC
{
    public class AssetAsIsSdiInfoRepository : RepositoryBase<Assetasissdiinfo>, IAssetAsIsSdiInfoRepository
    {
        ModelContext _modelContext;
        public AssetAsIsSdiInfoRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _modelContext = repositoryContext;
        }

        public void Detach()
        {

            var type = typeof(Assetasissdiinfo);
            var changedEntriesCopy = _modelContext.ChangeTracker.Entries()
                .Where(e => e.Entity.GetType() == type)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;


        }
    }
}
