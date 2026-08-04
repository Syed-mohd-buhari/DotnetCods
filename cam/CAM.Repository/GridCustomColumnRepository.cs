using CAM.Contracts.RepositoryContracts;
using CAM.Entities;
using CAM.Entities.Models;
using CAM.Entities.Models.Engine;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository
{
    public class GridCustomColumnRepository : RepositoryBase<Gridcustomcolumn>, IGridCustomColumnRepository
    {
        public GridCustomColumnRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        


    }
}