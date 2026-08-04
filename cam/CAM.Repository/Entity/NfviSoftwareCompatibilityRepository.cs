using CAM.Contracts.RepositoryContracts.Entity;
using CAM.Repository.NewRepositoryWrapper.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.Entity
{
    public class NfviSoftwareCompatibilityRepository : RepositoryBase<Nfvisoftwarecompatibility>, INfviSoftwareCompatibilityRepository
    {
        public NfviSoftwareCompatibilityRepository(ModelContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
