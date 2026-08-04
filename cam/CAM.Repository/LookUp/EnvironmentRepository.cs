using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
    public  class EnvironmentRepository : RepositoryBase<Environments>, IEnvironmentRepository
    {
        public EnvironmentRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Environments>> GetAllWithRelations()
        {
            return await FindAll().ToListAsync();
        }
    }
}
