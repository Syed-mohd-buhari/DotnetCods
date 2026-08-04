using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts;
using CAM.Entities;
using CAM.Entities.Models;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
   public class VNFDesignComponentRepository : RepositoryBaseNew<Vfndesigncomponents>, IVNFDesignComponentRepository
    {
        public VNFDesignComponentRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Vfndesigncomponents>> GetAllWithRelations()
        {
            return await FindAll()
                .ToListAsync();
        }
    }
}
