using CAM.Contracts.RepositoryContracts.ForeignIndex;
using CAM.Entities;
using CAM.Entities.Models.ForeignIndex;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAM.Repository.ForeignIndex
{
    public class FI_SessionRepository : RepositoryBase<FiSessions>, IFI_SessionsRepository
    {
        public FI_SessionRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<FiSessions>> GetAllWithRelations()
        {
            return await FindAll().ToListAsync();
        }


    }
}
