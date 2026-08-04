using CAM.Contracts.RepositoryContracts.Cross;
using CAM.Entities;
using CAM.Entities.Models.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class NetworkElementAsPlannedSubDomainSpocRepository : RepositoryBaseNew<Networkelementasplannedsubdomainspoc>, INetworkElementAsPlannedSubDomainSpocRepository
    {
        public NetworkElementAsPlannedSubDomainSpocRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}