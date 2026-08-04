using CAM.Contracts.RepositoryContracts.Base;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Linq;
using static CAM.Enum.ResourceTypeEnum;

namespace CAM.BusinessManager.ExtensionMethod.ComponentBag
{
    public static class ComponentBagExtensionMethod
    {
        public static string GetComponentBagDescription(IRepositoryWrapper _repositoryWrapper,long? componentSwBuildId)
        {

            var componentBagDescription = _repositoryWrapper.ComponentSoftwareBuildRepository.
            FindByCondition(x => x.Componentsoftwarebuildid == componentSwBuildId).Include(x=>x.Componentmanufacturer).FirstOrDefault();
                    
            if (componentBagDescription != null)
            {
                return $"{componentBagDescription.Componentmanufacturer.Componentmanufacturer}-" +
                                             $"{componentBagDescription.Componentmanufacturer.Componentname}-" +
                                             $"{componentBagDescription.Softwareversion}";
            }
            
            return string.Empty;
        }

        public static string GetComponentResourcekey(IRepositoryWrapper _repositoryWrapper, short opCoId, string componentName, long? dcfId)
        {
            try
            {

                var existResourceKey = _repositoryWrapper.ResourceKeyMaster.FindByCondition(x => x.Opcoid == opCoId && x.Elementname == componentName && x.Dcfid == dcfId
                && x.Resourcetypesid == (long)ResourceTypesKey.Component).FirstOrDefault();
                if(existResourceKey != null)
                {
                    return existResourceKey.Resourcekey;
                }
                return string.Empty;

            }
            catch(Exception  )
            {
                return string.Empty;
            }
        }

        public static string GetComponentDescription(Componentsoftwarebuilds componentSwBuilds)
        {
            return $"{componentSwBuilds.Componentmanufacturer.Componentmanufacturer}-" +
                                             $"{componentSwBuilds.Componentmanufacturer.Componentname}-" +
                                             $"{componentSwBuilds.Softwareversion}";
        }
    }
}
