using System.Linq;
using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
   public class BundleUpgradeInitiativeRepository : RepositoryBase<Bundleupgradeinitiatives>, IBundleUpgradeInitiativeRepository
    {
        public BundleUpgradeInitiativeRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        public IQueryable<Bundleupgradeinitiatives> GetAllWithRelations()
        {
            return FindAll();
        }
    }
}
