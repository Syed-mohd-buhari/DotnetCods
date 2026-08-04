using CAM.Contracts.RepositoryContracts.Cross;
using CAM.Entities;
using CAM.Entities.Models.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class NetworkElementAsPlannedEduSpocRepository : RepositoryBaseNew<Networkelementasplannededuspoc>, INetworkElementAsPlannedEduSpocRepository
    {
        public NetworkElementAsPlannedEduSpocRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}