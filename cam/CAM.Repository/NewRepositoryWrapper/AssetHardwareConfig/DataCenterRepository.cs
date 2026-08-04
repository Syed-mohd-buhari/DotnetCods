using OracleModels.DBContext;
using OracleModels.DBModels;
using CAM.Contracts.RepositoryContracts.AssetHardwareConfig;
namespace CAM.Repository.NewRepositoryWrapper.AssetHardwareConfig
{
    public class DataCenterRepository : RepositoryBaseNew<Datacenter>, IDataCenterRepository
    {
        ModelContextNew _context;
        public DataCenterRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }


    }
}
