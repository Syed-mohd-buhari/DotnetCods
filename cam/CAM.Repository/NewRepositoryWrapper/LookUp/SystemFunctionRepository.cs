using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models.Lookup;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class SystemFunctionRepository : RepositoryBaseNew<Systemfunctions>, ISystemFunctionRepository
    {
        public SystemFunctionRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Systemfunctions>> GetAllWithRelations()
        {
            return await FindAll().ToListAsync();
        }
    }
}
