using OracleModels.DBContext;
using OracleModels.DBModels;
using CAM.Contracts.RepositoryContracts.AssetHardwareConfig;

namespace CAM.Repository.NewRepositoryWrapper.AssetHardwareConfig
{
    public class AssetClusterRepository : RepositoryBaseNew<Assetcluster>, IAssetClusterRepository
    {
        ModelContextNew _context;
        public AssetClusterRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }


    }
}
