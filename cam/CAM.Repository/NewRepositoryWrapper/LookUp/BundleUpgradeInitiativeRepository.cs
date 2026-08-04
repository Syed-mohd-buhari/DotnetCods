using System.Linq;
using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
   public class BundleUpgradeInitiativeRepository : RepositoryBaseNew<Bundleupgradeinitiatives>, IBundleUpgradeInitiativeRepository
    {
        public BundleUpgradeInitiativeRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

        public IQueryable<Bundleupgradeinitiatives> GetAllWithRelations()
        {
            return FindAll();
        }
    }
}
