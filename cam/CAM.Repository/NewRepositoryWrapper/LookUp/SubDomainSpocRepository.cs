using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models.Lookup;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class SubDomainSpocRepository : RepositoryBaseNew<Subdomainspocs>, ISubDomainSpocRepository
    {
        public SubDomainSpocRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}