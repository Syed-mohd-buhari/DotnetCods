using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.Settings;
using CAM.Entities;
using CAM.Entities.Models.Lookup;
using CAM.Entities.Models.Settings;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.Settings
{
   public class SettingsUpdatePlannedActivityRepository : RepositoryBaseNew<Settingsupdateplannedactivity>, ISettingsUpdatePlannedActivityRepository
    {
       public SettingsUpdatePlannedActivityRepository(ModelContextNew repositoryContext) : base(repositoryContext)
       {
       }

       public async Task<IEnumerable<Settingsupdateplannedactivity>> GetAllWithRelations()
       {
           return await FindAll()
               .ToListAsync();
       }

    }
}
