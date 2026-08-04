using OracleModels.DBContext;
using OracleModels.DBModels;
using CAM.Contracts.RepositoryContracts.AssetHardwareConfig;

namespace CAM.Repository.AssetHardwareConfig
{
    public class AssetHardwareAncillaryRepository : RepositoryBase<Assethardwareancillary>, IAssetHardwareAncillaryRepository
    {
        ModelContext _context;
        public AssetHardwareAncillaryRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }


    }
}
