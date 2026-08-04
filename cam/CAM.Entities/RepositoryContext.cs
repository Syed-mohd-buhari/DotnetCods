using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;

namespace CAM.Entities
{
    public class RepositoryContext : ModelContext
    {
        public RepositoryContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
            
        }


    }
}