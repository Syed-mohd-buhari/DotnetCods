using OracleModels.DBContext;
using OracleModels.DBModels;
using CAM.Contracts.RepositoryContracts.AssetHardwareConfig;

namespace CAM.Repository.AssetHardwareConfig
{
    public class AssetClusterRepository : RepositoryBase<Assetcluster>, IAssetClusterRepository
    {
        ModelContext _context;
        public AssetClusterRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }


    }
}
