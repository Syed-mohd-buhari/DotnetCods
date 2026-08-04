using CAM.Contracts.RepositoryContracts;
using CAM.Contracts.RepositoryContracts.Cross;
using CAM.Entities;
using CAM.Entities.Models.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.Cross
{
    public class MajorSoftwareBuildNetworkFunctionRepository : RepositoryBaseNew<Majorsoftwarebuildnetworkfunction>, IMajorSoftwareBuildNetworkFunctionRepository
    {
        public MajorSoftwareBuildNetworkFunctionRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}
