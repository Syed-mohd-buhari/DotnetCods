using CAM.Contracts.RepositoryContracts.Base;
using System;
using System.Collections.Generic;
using System.Text;
using OracleModels.DBModels;
using System.Threading.Tasks;

namespace CAM.Contracts.RepositoryContracts.LookUp
{
    public interface ISubnetworkSystemFunctionsRepository : IRepositoryBase<Subnetwrokboundarysystemfunction>
    {
        Task<IEnumerable<Subnetwrokboundarysystemfunction>> GetAllWithRelations();
    }
}
