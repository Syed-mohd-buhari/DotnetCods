using OracleModels.DBContext;
using OracleModels.DBModels;
using CAM.Contracts.RepositoryContracts.AssetHardwareConfig;
namespace CAM.Repository.NewRepositoryWrapper.AssetHardwareConfig
{
    public class AssetHardwareAncillaryRepository : RepositoryBaseNew<Assethardwareancillary>, IAssetHardwareAncillaryRepository
    {
        ModelContextNew _context;
        public AssetHardwareAncillaryRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }


    }
}
