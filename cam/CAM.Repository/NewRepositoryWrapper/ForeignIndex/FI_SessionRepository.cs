using CAM.Contracts.RepositoryContracts.ForeignIndex;
using CAM.Entities;
using CAM.Entities.Models.ForeignIndex;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAM.Repository.NewRepositoryWrapper.ForeignIndex
{
    public class FI_SessionRepository : RepositoryBaseNew<FiSessions>, IFI_SessionsRepository
    {
        public FI_SessionRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<FiSessions>> GetAllWithRelations()
        {
            return await FindAll().ToListAsync();
        }


    }
}
