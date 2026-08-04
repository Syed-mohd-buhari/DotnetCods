using System.Linq;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.Entities.Models;
using OracleModels.DBModels;

namespace CAM.Contracts.RepositoryContracts
{
    public interface INFVITransitionRepository : IRepositoryBase<Nfvitransitions>
    {
        IQueryable<Nfvitransitions> GetAllWithRelations();
    }
}
