using CAM.Contracts.RepositoryContracts.Entity;
using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class MajorHwBuidlsDesignContactsRepository : RepositoryBaseNew<Majorhwbuildsdesigncontacts>, IMajorHwBuidlsDesignContactsRepository
    {
        public MajorHwBuidlsDesignContactsRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {

        }
    }
}
