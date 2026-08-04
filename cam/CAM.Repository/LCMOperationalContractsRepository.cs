using CAM.Contracts.RepositoryContracts.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Repository
{
    public class LCMOperationalContractsRepository : RepositoryBase<Lcmoperationalcontracts>, ILCMOperationalContractsRepository
    {
        public LCMOperationalContractsRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
