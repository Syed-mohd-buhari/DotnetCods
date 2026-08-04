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
   public class BusinessContinuityMethodRepository : RepositoryBase<Businesscontinuitymethod>, IBusinessContinuityMethodRepository
    {
       public BusinessContinuityMethodRepository(ModelContext repositoryContext) : base(repositoryContext)
       {
       }

       public async Task<IEnumerable<Businesscontinuitymethod>> GetAllWithRelations()
       {
           return await FindAll()
               .ToListAsync();
       }
    }
}
