using CAM.Contracts.RepositoryContracts.Base;
using CAM.Contracts.RepositoryContracts.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Repository.Cross
{
    public class DesignAspectNetworkFunctionRepository : RepositoryBase<Designaspectsnetworkfunctions>, IDesignAspectNetworkFunctionRepository
    {
        public DesignAspectNetworkFunctionRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
