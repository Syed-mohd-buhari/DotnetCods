using CAM.Contracts.RepositoryContracts.AssetHardwareConfig;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.AssetHardwareConfig
{
    public class AssetCapacityInfoRepository : RepositoryBase<Assetcapacityinfo>, IAssetCapacityInfoRepository
    {
        ModelContext _context;
        public AssetCapacityInfoRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }


    }
}
