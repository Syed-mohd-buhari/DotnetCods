using OracleModels.DBContext;
using OracleModels.DBModels;
using CAM.Contracts.RepositoryContracts.AssetHardwareConfig;
namespace CAM.Repository.AssetHardwareConfig
{
    public class AssetClusterTypeRepository : RepositoryBase<Assetclustertype>, IAssetClusterTypeRepository
    {
        ModelContext _context;
        public AssetClusterTypeRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }


    }
}
