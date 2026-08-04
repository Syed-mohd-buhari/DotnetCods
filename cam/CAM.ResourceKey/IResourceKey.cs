using CAM.Contracts.RepositoryContracts.Base;
using CAM.Contracts.RepositoryContracts.Entity;
using CAM.DataTransferObjects;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.ResourcesKey
{
    public interface IResourceKey
    {
        string GenerateRandomResourceKey(IRepositoryWrapper repositoryWrapper,int lcmId);
        bool IsResourceKeyExist(IRepositoryWrapper repositoryWrapper, string RandomValue);
    }
}
