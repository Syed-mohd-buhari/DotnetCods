using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Repository.NewRepositoryWrapper.Entity
{
    public class HardwareConfigurationRepository : RepositoryBaseNew<Hardwareconfiguration> ,IHardwareConfigurationRepository
    {
        public HardwareConfigurationRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}
