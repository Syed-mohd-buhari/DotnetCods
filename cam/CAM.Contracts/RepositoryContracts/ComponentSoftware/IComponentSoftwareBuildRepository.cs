using System.Linq;
using CAM.Contracts.RepositoryContracts.Base; 
using OracleModels.DBModels;

namespace CAM.Contracts.RepositoryContracts.ComponentSoftware
{
    public interface IComponentSoftwareBuildRepository : IRepositoryBase<Componentsoftwarebuilds>
    {
        //IQueryable<Componentsoftwarebuilds> GetAllWithRelations();
        //void Detach();
    }
}