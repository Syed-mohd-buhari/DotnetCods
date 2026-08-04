using CAM.Contracts.RepositoryContracts.Entity;
using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class MajorSwBuidlsDesignContactsRepository : RepositoryBaseNew<Majorswbuildsdesigncontacts>, IMajorSwBuidlsDesignContactsRepository
    {
        public MajorSwBuidlsDesignContactsRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {

        }
    }
}
