using CAM.Contracts.RepositoryContracts.Cross;
using CAM.Contracts.RepositoryContracts.LookUp;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Repository.LookUp
{
    public class LocationDeploymentTypeRepository : RepositoryBase<Locationdeploymenttypes>, ILocationDeploymentTypeRepository
    {
        public LocationDeploymentTypeRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }
        public async Task<IEnumerable<Locationdeploymenttypes>> GetAllWithRelations()
        {
            return await FindAll().ToListAsync();
        }
    }
}
