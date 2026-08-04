using CAM.Contracts.RepositoryContracts.Base;
using CAM.Contracts.RepositoryContracts.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Repository.NewRepositoryWrapper.Cross
{
    public class DesignAspectSupportedServiceRepository : RepositoryBaseNew<Designaspectssupportedsvr>, IDesignAspectSupportedServiceRepository
    {
        public DesignAspectSupportedServiceRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}
