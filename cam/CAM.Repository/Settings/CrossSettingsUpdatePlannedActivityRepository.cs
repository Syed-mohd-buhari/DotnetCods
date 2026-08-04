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

namespace CAM.Repository.Settings
{
    public class CrossSettingsUpdatePlannedActivityRepository : RepositoryBase<Crosssettingsupdateplannedactivity>, ICrossSettingsUpdatePlannedActivityRepository
    {
        public CrossSettingsUpdatePlannedActivityRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Crosssettingsupdateplannedactivity>> GetAllWithRelations()
        {
            return await FindAll()
                .ToListAsync();
        }
    }
}
