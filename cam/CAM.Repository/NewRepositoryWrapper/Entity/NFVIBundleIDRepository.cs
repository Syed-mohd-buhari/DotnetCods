using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.Entity;
using CAM.Entities;
using CAM.Entities.Models;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.Entity
{
    public class NFVIBundleIDRepository : RepositoryBaseNew<Nfvibundleids>, INFVIBundleIDRepository
    {
        public NFVIBundleIDRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Nfvibundleids>> GetAllWithRelations()
        {
            return await FindAll().ToListAsync();
        }
    }
}
