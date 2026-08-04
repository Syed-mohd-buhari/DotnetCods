using CAM.Contracts.RepositoryContracts.Base;
using CAM.Contracts.RepositoryContracts.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Repository.Cross
{
    public class DesignAspectSupportedServiceRepository : RepositoryBase<Designaspectssupportedsvr>, IDesignAspectSupportedServiceRepository
    {
        public DesignAspectSupportedServiceRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
