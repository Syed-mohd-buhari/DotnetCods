using CAM.Contracts.RepositoryContracts.Entity;
using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.Entity
{
    public class MajorHwBuidlsDesignContactsRepository : RepositoryBase<Majorhwbuildsdesigncontacts>, IMajorHwBuidlsDesignContactsRepository
    {
        public MajorHwBuidlsDesignContactsRepository(ModelContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
