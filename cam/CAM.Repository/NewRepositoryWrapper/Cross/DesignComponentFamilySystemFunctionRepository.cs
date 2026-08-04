using CAM.Contracts.RepositoryContracts;
using CAM.Contracts.RepositoryContracts.Cross;
using CAM.Entities;
using CAM.Entities.Models.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.Cross
{
    public class DesignComponentFamilySystemFunctionRepository : RepositoryBaseNew<Designcomponentfamilysystemfunction>, IDesignComponentFamilySystemFunctionRepository
    {
        public DesignComponentFamilySystemFunctionRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}