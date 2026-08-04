using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models.Lookup;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
   public class EndOfSupportContractRepository : RepositoryBaseNew<EndOfSupportContract>, IEndOfSupportContractRepository
    {
        public EndOfSupportContractRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<EndOfSupportContract>> GetAllWithRelations()
        {
            return await FindAll().ToListAsync();
        }
    }
}
