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
    public class ProjectPlanRepository : RepositoryBaseNew<Projectsplan> ,IProjectPlanRepository
    {
        public ProjectPlanRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}
