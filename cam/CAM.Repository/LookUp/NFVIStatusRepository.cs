using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts;
using CAM.Entities;
using CAM.Entities.Models.Lookup;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
   public class NFVIStatusRepository : RepositoryBase<Nfvistatuses>, INFVIStatusRepository
    {
        public NFVIStatusRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Nfvistatuses>> GetAllWithRelations()
        {
            return await FindAll().ToListAsync();
        }

    }
}
