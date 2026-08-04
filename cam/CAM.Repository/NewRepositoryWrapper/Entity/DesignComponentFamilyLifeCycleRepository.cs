using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.Entity;
using CAM.Entities;
using CAM.Entities.Models;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.Entity
{
    public class DesignComponentFamilyLifeCycleRepository : RepositoryBaseNew<Dcflifecycle>, IDesignComponentFamilyLifeCycleRepository
    {
        public DesignComponentFamilyLifeCycleRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {

        }
    }
}
