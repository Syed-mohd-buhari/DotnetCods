using System.Linq;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.Entities.Models;
using OracleModels.DBModels;

namespace CAM.Contracts.RepositoryContracts.LookUp
{
   public interface IBundleUpgradeInitiativeRepository : IRepositoryBase<Bundleupgradeinitiatives>
    {
        IQueryable<Bundleupgradeinitiatives> GetAllWithRelations();
    }
}
