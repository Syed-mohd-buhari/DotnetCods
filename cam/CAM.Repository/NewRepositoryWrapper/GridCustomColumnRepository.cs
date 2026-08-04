using CAM.Contracts.RepositoryContracts;
using CAM.Entities;
using CAM.Entities.Models;
using CAM.Entities.Models.Engine;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper
{
    public class GridCustomColumnRepository : RepositoryBaseNew<Gridcustomcolumn>, IGridCustomColumnRepository
    {
        public GridCustomColumnRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

        


    }
}