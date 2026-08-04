using CAM.Contracts.RepositoryContracts.Cross;
using CAM.Entities;
using CAM.Entities.Models.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
    public class NetworkElementAsPlannedEduSpocRepository : RepositoryBase<Networkelementasplannededuspoc>, INetworkElementAsPlannedEduSpocRepository
    {
        public NetworkElementAsPlannedEduSpocRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}