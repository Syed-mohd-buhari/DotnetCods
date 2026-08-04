using CAM.Contracts.RepositoryContracts.Entity;
using CAM.Repository.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Repository.NewRepositoryWrapper.Entity
{
    public class PlannedActivityTypesRepository : RepositoryBaseNew<Plannedactivitytypes>, IPlannedActivityTypesRepository
    {
        public PlannedActivityTypesRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {

        }

    }
}
