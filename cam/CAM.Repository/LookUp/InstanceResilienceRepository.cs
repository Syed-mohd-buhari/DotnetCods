using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models.Lookup;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
   public class InstanceResilienceRepository : RepositoryBase<Instanceresilience>, IInstanceResilienceRepository
    {
       public InstanceResilienceRepository(ModelContext repositoryContext) : base(repositoryContext)
       {
       }

       public async Task<IEnumerable<Instanceresilience>> GetAllWithRelations()
       {
           return await FindAll()
               .ToListAsync();
       }
    }
}
