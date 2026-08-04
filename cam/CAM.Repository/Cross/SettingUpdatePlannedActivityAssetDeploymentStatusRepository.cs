using CAM.Contracts.RepositoryContracts.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Repository.Cross
{
    public class SettingUpdatePlannedActivityAssetDeploymentStatusRepository : RepositoryBase<Settingupdateplannedactivityassetdeploymentstatus>, ISettingUpdatePlannedActivityAssetDeploymentStatusRepository
    {
        public SettingUpdatePlannedActivityAssetDeploymentStatusRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
