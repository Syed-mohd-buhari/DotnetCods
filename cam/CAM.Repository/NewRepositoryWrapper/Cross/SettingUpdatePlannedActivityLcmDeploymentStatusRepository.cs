using CAM.Contracts.RepositoryContracts.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Repository.NewRepositoryWrapper.Cross
{
    public class SettingUpdatePlannedActivityLcmDeploymentStatusRepository : RepositoryBaseNew<Settingupdateplannedactivitylcmdeploymentstatus>, ISettingUpdatePlannedActivityLcmDeploymentStatusRepository
    {
        public SettingUpdatePlannedActivityLcmDeploymentStatusRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}
