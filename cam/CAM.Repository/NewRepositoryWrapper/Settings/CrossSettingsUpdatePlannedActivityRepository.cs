using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.Settings;
using CAM.Entities;
using CAM.Entities.Models.Settings;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.Settings
{
    public class CrossSettingsUpdatePlannedActivityRepository : RepositoryBaseNew<Crosssettingsupdateplannedactivity>, ICrossSettingsUpdatePlannedActivityRepository
    {
        public CrossSettingsUpdatePlannedActivityRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Crosssettingsupdateplannedactivity>> GetAllWithRelations()
        {
            return await FindAll()
                .ToListAsync();
        }
    }
}
