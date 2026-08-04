using CAM.Contracts.RepositoryContracts.Base;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Contracts.RepositoryContracts.LookUp
{
    public interface IClassRepository : IRepositoryBase<Classes>
    {
        IDictionary<int, string> GetClassesByCategoryId(int categoryId);
    }
}
