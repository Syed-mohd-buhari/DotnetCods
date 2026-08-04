using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class LcmAncillaryDataRepository : RepositoryBaseNew<Lcmancillarydata>, ILcmAncillaryDataRepository
    {
        public LcmAncillaryDataRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

    }
}
