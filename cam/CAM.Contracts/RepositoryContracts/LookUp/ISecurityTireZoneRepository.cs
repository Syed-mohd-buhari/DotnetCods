using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
namespace CAM.Contracts.RepositoryContracts.LookUp
{
    public interface ISecurityTireZoneRepository : IRepositoryBase<Securitytirezone>
    {
        Task<IEnumerable<Securitytirezone>> GetAllWithRelations();
    }
}