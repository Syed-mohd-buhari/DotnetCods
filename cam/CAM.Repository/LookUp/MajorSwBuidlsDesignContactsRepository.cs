using CAM.Contracts.RepositoryContracts.Entity;
using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.Entity
{
    public class MajorSwBuidlsDesignContactsRepository : RepositoryBase<Majorswbuildsdesigncontacts>, IMajorSwBuidlsDesignContactsRepository
    {
        public MajorSwBuidlsDesignContactsRepository(ModelContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
