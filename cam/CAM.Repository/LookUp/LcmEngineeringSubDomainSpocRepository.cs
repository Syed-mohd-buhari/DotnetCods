using CAM.Contracts.RepositoryContracts;
using CAM.Contracts.RepositoryContracts.Cross;
using CAM.Entities;
using CAM.Entities.Models.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
    public class LcmEngineeringSubDomainSpocRepository : RepositoryBase<Lcmengineeringsubdomainspoc>, ILcmEngineeringSubDomainSpocRepository
    {
        public LcmEngineeringSubDomainSpocRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}