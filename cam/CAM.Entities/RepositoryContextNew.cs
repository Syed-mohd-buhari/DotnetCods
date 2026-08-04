using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;

namespace CAM.Entities
{
    public class RepositoryContextNew : ModelContextNew
    {
        public RepositoryContextNew(DbContextOptions<ModelContextNew> options)
            : base(options)
        {
            
        }


    }
}