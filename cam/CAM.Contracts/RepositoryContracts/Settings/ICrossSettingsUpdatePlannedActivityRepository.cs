using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.Entities.Models.Settings;
using OracleModels.DBModels;

namespace CAM.Contracts.RepositoryContracts.Settings
{
    public interface ICrossSettingsUpdatePlannedActivityRepository   : IRepositoryBase<Crosssettingsupdateplannedactivity>
    {
        Task<IEnumerable<Crosssettingsupdateplannedactivity>> GetAllWithRelations();
    }
}
