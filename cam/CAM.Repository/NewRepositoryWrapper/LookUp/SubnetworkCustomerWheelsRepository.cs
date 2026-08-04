using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;
namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class SubnetworkCustomerWheelsRepository : RepositoryBaseNew<Subnetworkboundarycustomerwheel>, ISubnetworkCustomerWheelsRepository
    {
        public SubnetworkCustomerWheelsRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Subnetworkboundarycustomerwheel>> GetAllWithRelations()
        {
            return await FindAll().ToListAsync();
        }
    }
}
