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

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
   public class LicenseModelRepository : RepositoryBaseNew<Licensemodels>, ILicenseModelRepository
    {
       public LicenseModelRepository(ModelContextNew repositoryContext) : base(repositoryContext)
       {
       }

       public async Task<IEnumerable<Licensemodels>> GetAllWithRelations()
       {
           return await FindAll()
               .ToListAsync();
       }
    }
}
