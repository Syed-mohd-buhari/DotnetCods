using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models.Lookup;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;
using Environment = System.Environment;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class DeploymentTypeRepository : RepositoryBaseNew<Deploymenttypes>, IDeploymentTypeRepository
    {
        public DeploymentTypeRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Deploymenttypes>> GetAllWithRelations()
        {
            return await FindAll().ToListAsync();
        }
    }
}
