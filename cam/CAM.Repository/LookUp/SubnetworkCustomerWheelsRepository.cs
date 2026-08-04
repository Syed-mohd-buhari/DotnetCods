using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;
namespace CAM.Repository.LookUp
{
    public class SubnetworkCustomerWheelsRepository : RepositoryBase<Subnetworkboundarycustomerwheel>, ISubnetworkCustomerWheelsRepository
    {
        public SubnetworkCustomerWheelsRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Subnetworkboundarycustomerwheel>> GetAllWithRelations()
        {
            return await FindAll().ToListAsync();
        }
    }
}
