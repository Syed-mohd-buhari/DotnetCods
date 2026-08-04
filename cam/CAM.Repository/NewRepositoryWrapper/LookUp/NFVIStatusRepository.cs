using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts;
using CAM.Entities;
using CAM.Entities.Models.Lookup;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
   public class NFVIStatusRepository : RepositoryBaseNew<Nfvistatuses>, INFVIStatusRepository
    {
        public NFVIStatusRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Nfvistatuses>> GetAllWithRelations()
        {
            return await FindAll().ToListAsync();
        }

    }
}
