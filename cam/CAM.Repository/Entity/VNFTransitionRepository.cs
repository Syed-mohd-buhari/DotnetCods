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
    public class VNFTransitionRepository : RepositoryBase<Vnftransitions>, IVNFTransitionRepository
    {
        public VNFTransitionRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }
        public async Task<IEnumerable<Vnftransitions>> GetAllWithRelations()
        {
            return await FindAll().ToListAsync();
        }
    }
}
