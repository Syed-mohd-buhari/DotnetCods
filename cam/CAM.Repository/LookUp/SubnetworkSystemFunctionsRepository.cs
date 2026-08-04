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
    public class SubnetworkSystemFunctionsRepository : RepositoryBase<Subnetwrokboundarysystemfunction>, ISubnetworkSystemFunctionsRepository
    {
        public SubnetworkSystemFunctionsRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Subnetwrokboundarysystemfunction>> GetAllWithRelations()
        {
            return await FindAll().ToListAsync();
        }
    }
}
