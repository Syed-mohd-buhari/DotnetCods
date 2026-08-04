using CAM.Contracts.RepositoryContracts.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Repository.NewRepositoryWrapper.Cross
{
    public class SettingUpdatePlannedActivityAssetDeploymentStatusRepository : RepositoryBaseNew<Settingupdateplannedactivityassetdeploymentstatus>, ISettingUpdatePlannedActivityAssetDeploymentStatusRepository
    {
        public SettingUpdatePlannedActivityAssetDeploymentStatusRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}
