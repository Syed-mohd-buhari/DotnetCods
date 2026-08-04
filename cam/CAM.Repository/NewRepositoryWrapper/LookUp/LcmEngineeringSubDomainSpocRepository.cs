using CAM.Contracts.RepositoryContracts;
using CAM.Contracts.RepositoryContracts.Cross;
using CAM.Entities;
using CAM.Entities.Models.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class LcmEngineeringSubDomainSpocRepository : RepositoryBaseNew<Lcmengineeringsubdomainspoc>, ILcmEngineeringSubDomainSpocRepository
    {
        public LcmEngineeringSubDomainSpocRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}