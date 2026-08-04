using OracleModels.DBContext;
using OracleModels.DBModels;
using CAM.Contracts.RepositoryContracts.AssetHardwareConfig;

namespace CAM.Repository.NewRepositoryWrapper.AssetHardwareConfig
{
    public class AssetCapacityInfoRepository : RepositoryBaseNew<Assetcapacityinfo>, IAssetCapacityInfoRepository
    {
        ModelContextNew _context;
        public AssetCapacityInfoRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }


    }
}


