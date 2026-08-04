using CAM.Contracts.RepositoryContracts.VBom;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.VBom
{
    public class VnfInfoRepository : RepositoryBase<Vnfinfo>, IVnfInfoRepository
    {
        ModelContext _context;
        public VnfInfoRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }


    }
}

