using OracleModels.DBContext;
using OracleModels.DBModels;
using CAM.Contracts.RepositoryContracts.Entity;
using CAM.Repository.Entity;

namespace CAM.Repository.NewRepositoryWrapper.Entity
{
    public class NfviSoftwareCompatibilityRepository : RepositoryBaseNew<Nfvisoftwarecompatibility>, INfviSoftwareCompatibilityRepository
    {
        public NfviSoftwareCompatibilityRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}
