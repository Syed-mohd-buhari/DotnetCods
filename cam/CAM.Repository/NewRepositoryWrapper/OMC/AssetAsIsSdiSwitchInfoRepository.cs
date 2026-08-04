using CAM.Contracts.RepositoryContracts.OMC;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Repository.NewRepositoryWrapper.OMC
{
    public class AssetAsIsSdiSwitchInfoRepository : RepositoryBaseNew<Assetasissdiswitchinfo>, IAssetAsIsSdiSwitchInfoRepository
    {
        ModelContextNew _modelContext;
        public AssetAsIsSdiSwitchInfoRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _modelContext = repositoryContext;
        }

        public void Detach()
        {

            var type = typeof(Assetasissdiswitchinfo);
            var changedEntriesCopy = _modelContext.ChangeTracker.Entries()
                .Where(e => e.Entity.GetType() == type)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;


        }
    }
}
