using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts;
using CAM.Entities;
using CAM.Entities.Models;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
   public class VNFDesignComponentRepository : RepositoryBase<Vfndesigncomponents>, IVNFDesignComponentRepository
    {
        public VNFDesignComponentRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Vfndesigncomponents>> GetAllWithRelations()
        {
            return await FindAll()
                .ToListAsync();
        }
    }
}
