using CAM.Contracts.RepositoryContracts.Cross;
using CAM.Entities;
using CAM.Entities.Models.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
    public class NetworkElementAsPlannedSubDomainSpocRepository : RepositoryBase<Networkelementasplannedsubdomainspoc>, INetworkElementAsPlannedSubDomainSpocRepository
    {
        public NetworkElementAsPlannedSubDomainSpocRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}