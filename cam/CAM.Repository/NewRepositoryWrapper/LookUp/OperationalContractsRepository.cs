using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public partial class OperationalContractsRepository : RepositoryBaseNew<Operationalcontracts>, IOperationalContractsRepository
    {
        public OperationalContractsRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Operationalcontracts>> GetAllWithRelations()
        {
            return await FindAll()
                .ToListAsync();
        }
    }
}
