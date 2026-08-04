using CAM.Contracts.RepositoryContracts;
using CAM.Contracts.RepositoryContracts.Cross;
using CAM.Entities;
using CAM.Entities.Models.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.Cross
{
    public class MajorSoftwareBuildNetworkFunctionRepository : RepositoryBase<Majorsoftwarebuildnetworkfunction>, IMajorSoftwareBuildNetworkFunctionRepository
    {
        public MajorSoftwareBuildNetworkFunctionRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
