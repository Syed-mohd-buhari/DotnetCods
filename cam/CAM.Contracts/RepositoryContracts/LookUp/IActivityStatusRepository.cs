using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;

namespace CAM.Contracts.RepositoryContracts.LookUp
{
    public interface IActivityStatusRepository : IRepositoryBase<Activitystatuses>
    {
        IQueryable<Activitystatuses> GetAllWithRelations();

        IQueryable<Activitystatuses> GetAll();
    }
}