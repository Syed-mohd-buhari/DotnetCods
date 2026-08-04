using CAM.Contracts.RepositoryContracts.ForeignIndex;
using CAM.Entities;
using CAM.Entities.Models.ForeignIndex;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAM.Repository.NewRepositoryWrapper.ForeignIndex
{
    public class FI_SystemTypesRepository : RepositoryBaseNew<FiSystemtypes>, IFI_SystemTypesRepository
    {
        public FI_SystemTypesRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<FiSystemtypes>> GetAllWithRelations()
        {
            return await FindAll().ToListAsync();
        }


    }
}
