using CAM.Contracts.RepositoryContracts.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Repository.Cross
{
    public class SettingUpdatePlannedActivityLcmDeploymentStatusRepository : RepositoryBase<Settingupdateplannedactivitylcmdeploymentstatus>, ISettingUpdatePlannedActivityLcmDeploymentStatusRepository
    {
        public SettingUpdatePlannedActivityLcmDeploymentStatusRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
