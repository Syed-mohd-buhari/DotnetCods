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
    public class OpCoRepository : RepositoryBase<Opcos>, IOpCoRepository
    {
        public OpCoRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Opcos>> GetAllWithRelations()
        {
            return await FindAll()
                .ToListAsync();
        }


    }
}
