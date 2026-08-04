using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models.Lookup;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
    public class ReasonCheckboxResourceRepository : RepositoryBase<Reasoncheckboxresources>, IReasonCheckboxResourceRepository
    {
        public ReasonCheckboxResourceRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

       
        
    }
}