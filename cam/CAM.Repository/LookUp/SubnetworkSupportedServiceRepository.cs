using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
namespace CAM.Repository.LookUp
{
    public class SubnetworkSupportedServiceRepository : RepositoryBase<Subnetworksupportedsvr>, ISubnetworkSupportedServiceRepository
    {
        public SubnetworkSupportedServiceRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Subnetworksupportedsvr>> GetAllWithRelations()
        {
            return await FindAll().ToListAsync();
        }
    }
}
