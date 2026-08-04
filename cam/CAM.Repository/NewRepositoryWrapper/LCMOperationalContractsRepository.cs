using CAM.Contracts.RepositoryContracts.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Repository.NewRepositoryWrapper
{
    public class LCMOperationalContractsRepository : RepositoryBaseNew<Lcmoperationalcontracts>, ILCMOperationalContractsRepository
    {
        public LCMOperationalContractsRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}
