using CAM.Contracts.RepositoryContracts.OMC;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Repository.OMC
{
    public class AssetMapInfoRepository : RepositoryBase<Assetmapinfo>, IAssetMapInfoRepository
    {
        ModelContext _modelContext;
        public AssetMapInfoRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _modelContext = repositoryContext;
        }

        public void Detach()
        {

            var type = typeof(Assetmapinfo);
            var changedEntriesCopy = _modelContext.ChangeTracker.Entries()
                .Where(e => e.Entity.GetType() == type)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;


        }
    }
}
