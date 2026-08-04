using CAM.Contracts.RepositoryContracts.Base;
using CAM.Contracts.RepositoryContracts.Entity;
using CAM.DataTransferObjects;
using OracleModels.DBModels;
using System;

namespace CAM.ResourcesKey
{
    public class ResourceKey : IResourceKey
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public ResourceKey(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        private Random random = new Random();
        public string GenerateRandomResourceKey(IRepositoryWrapper repositoryWrapper, int resourceId)
        {
            int length = 7;
            string randomString = "";
            const string possibleChars = "ABCDEF0123456789";
            do
            {
                randomString = resourceId.ToString() + new string(Enumerable.Repeat(possibleChars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
            } while (IsResourceKeyExist(repositoryWrapper, randomString));
           return randomString;

        }

        public bool IsResourceKeyExist(IRepositoryWrapper repositoryWrapper, string randomValue)
        {
            
            var ResourceMaster = repositoryWrapper.ResourceKeyMaster.FindByCondition(x=>x.Resourcekey == randomValue);
            if (ResourceMaster.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}