using CAM.Contracts.RepositoryContracts.Cross;
using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Repository.NewRepositoryWrapper.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class LcmDeploymentStatusRepository : RepositoryBaseNew<Lcmdeploymentstatus>, ILcmDeploymentStatusRepository
    {
        public LcmDeploymentStatusRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
        public async Task<IEnumerable<Lcmdeploymentstatus>> GetAllWithRelations()
        {
            return await FindAll()
                .ToListAsync();
        }
    }
}
