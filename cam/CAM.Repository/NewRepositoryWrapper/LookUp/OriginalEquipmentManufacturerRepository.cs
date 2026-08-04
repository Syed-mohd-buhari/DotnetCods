using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models.Lookup;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class OriginalEquipmentManufacturerRepository : RepositoryBaseNew<Originalequipmentmanufacturers>, IOriginalEquipmentManufacturerRepository
    {

        ModelContextNew _context;

        public OriginalEquipmentManufacturerRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }


        public void Detach()
        {

            var data = typeof(Originalequipmentmanufacturers);
            var changedEntriesCopy = _context.ChangeTracker.Entries()
                .Where(e => e.Entity.GetType() == data)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }

    }
}
