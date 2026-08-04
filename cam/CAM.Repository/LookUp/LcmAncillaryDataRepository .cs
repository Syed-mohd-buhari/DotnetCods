using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
    public class LcmAncillaryDataRepository : RepositoryBase<Lcmancillarydata>, ILcmAncillaryDataRepository
    {
        public LcmAncillaryDataRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

    }
}
