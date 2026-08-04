using CAM.Contracts.RepositoryContracts.Base;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Contracts.RepositoryContracts.LookUp
{
    public interface ILocationDeploymentTypeRepository: IRepositoryBase<Locationdeploymenttypes>
    {
        Task<IEnumerable<Locationdeploymenttypes>> GetAllWithRelations();
    }
}
