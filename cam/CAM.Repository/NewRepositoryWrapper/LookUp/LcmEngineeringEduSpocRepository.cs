using CAM.Contracts.RepositoryContracts;
using CAM.Contracts.RepositoryContracts.Cross;
using CAM.Entities;
using CAM.Entities.Models.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class LcmEngineeringEduSpocRepository : RepositoryBaseNew<Lcmengineeringeduspoc>, ILcmEngineeringEduSpocRepository
    {
        public LcmEngineeringEduSpocRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}