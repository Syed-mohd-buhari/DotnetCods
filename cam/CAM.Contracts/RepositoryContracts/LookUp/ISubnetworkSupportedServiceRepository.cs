using CAM.Contracts.RepositoryContracts.Base;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Contracts.RepositoryContracts.LookUp
{
    public interface ISubnetworkSupportedServiceRepository : IRepositoryBase<Subnetworksupportedsvr>
    {
        Task<IEnumerable<Subnetworksupportedsvr>> GetAllWithRelations();
    }
}
