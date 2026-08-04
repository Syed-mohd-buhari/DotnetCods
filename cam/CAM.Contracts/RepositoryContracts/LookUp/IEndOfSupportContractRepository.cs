using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.Entities.Models.Lookup;

namespace CAM.Contracts.RepositoryContracts.LookUp
{
    public interface IEndOfSupportContractRepository : IRepositoryBase<EndOfSupportContract>
    {
        Task<IEnumerable<EndOfSupportContract>> GetAllWithRelations();
    
    }
}
