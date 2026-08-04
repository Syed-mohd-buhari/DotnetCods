using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;

namespace CAM.Contracts.RepositoryContracts.LookUp
{
    public interface INetworkFunctionRepository : IRepositoryBase<Networkfunctions>
    {
        Task<IEnumerable<Networkfunctions>> GetAllWithRelations();
    }
}
