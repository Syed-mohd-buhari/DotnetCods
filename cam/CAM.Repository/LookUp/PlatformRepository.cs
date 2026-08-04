using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models.Lookup;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
    public class PlatformRepository : RepositoryBase<Platforms>, IPlatformRepository
    {
        public PlatformRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Platforms>> GetAllWithRelations()
        {
            return await FindAll().ToListAsync();
        }
    }
}
