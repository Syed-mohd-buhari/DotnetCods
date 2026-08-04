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

namespace CAM.Repository.LookUp
{
    public class DeploymentTypeRepository : RepositoryBase<Deploymenttypes>, IDeploymentTypeRepository
    {
        public DeploymentTypeRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Deploymenttypes>> GetAllWithRelations()
        {
            return await FindAll().ToListAsync();
        }
    }
}
