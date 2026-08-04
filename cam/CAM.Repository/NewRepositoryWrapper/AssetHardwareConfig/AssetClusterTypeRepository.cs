using OracleModels.DBContext;
using OracleModels.DBModels;
using CAM.Contracts.RepositoryContracts.AssetHardwareConfig;
namespace CAM.Repository.NewRepositoryWrapper.AssetHardwareConfig
{
    public class AssetClusterTypeRepository : RepositoryBaseNew<Assetclustertype>, IAssetClusterTypeRepository
    {
        ModelContextNew _context;
        public AssetClusterTypeRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }


    }
}
