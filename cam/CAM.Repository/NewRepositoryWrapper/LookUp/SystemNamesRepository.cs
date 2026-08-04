using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class SystemNamesRepository : RepositoryBaseNew<Systemnames>, ISystemNamesRepository
    {
        public SystemNamesRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}
