using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System.Collections.Generic;
using System.Linq;

namespace CAM.Repository.LookUp
{
    public class NetworkElementAsPlannedRepository : RepositoryBase<Networkelementsasplanned>, INetworkElementAsPlannedRepository
    {
        public NetworkElementAsPlannedRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        public IDictionary<long, string> GetAssetsByOpcoIdAndDcfId(short opcoId, long dcfId)
        {
            return ModelContext.Networkelementsasplanned
                .Where(x => x.Opcoid == opcoId && x.Designcomponent.Designcomponentfamilyid == dcfId)
                .ToDictionary(x => x.Networkelementasplannedid, x => x.Elementname);
        }
    }
}