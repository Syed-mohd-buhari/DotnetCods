using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.Entity;
using CAM.Entities;
using CAM.Entities.Models;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.Entity
{
    public class NFVIBundleIDRepository : RepositoryBase<Nfvibundleids>, INFVIBundleIDRepository
    {
        public NFVIBundleIDRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Nfvibundleids>> GetAllWithRelations()
        {
            return await FindAll().ToListAsync();
        }
    }
}
