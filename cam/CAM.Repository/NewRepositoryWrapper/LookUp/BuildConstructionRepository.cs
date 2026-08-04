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
   public class BuildConstructionRepository : RepositoryBaseNew<Buildconstructions>, IBuildConstructionRepository
    {
        public BuildConstructionRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Buildconstructions>> GetAllWithRelations()
        {
            return await FindAll().ToListAsync();
        }
    }
}
