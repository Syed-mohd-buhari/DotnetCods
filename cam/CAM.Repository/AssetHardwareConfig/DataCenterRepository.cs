using OracleModels.DBContext;
using OracleModels.DBModels;
using CAM.Contracts.RepositoryContracts.AssetHardwareConfig;

namespace CAM.Repository.AssetHardwareConfig
{
    public class DataCenterRepository : RepositoryBase<Datacenter>, IDataCenterRepository
    {
        ModelContext _context;
        public DataCenterRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }


    }
}
