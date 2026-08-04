using CAM.Contracts.RepositoryContracts.Base;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System.Collections.Generic;

namespace CAM.Contracts.RepositoryContracts.LookUp
{
    public interface INetworkElementAsPlannedRepository : IRepositoryBase<Networkelementsasplanned>
    {
        IDictionary<long, string> GetAssetsByOpcoIdAndDcfId(short opcoId,long dcfId);
    }
}