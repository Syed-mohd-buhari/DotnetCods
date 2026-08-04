using CAM.Contracts.RepositoryContracts.Base;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAM.BusinessManager.ExtensionMethod.NetworkElementAsPlanned
{
    public static class NetworkElementAsPlannedMethod
    {
        public static string toDescription(this Entities.Models.NetworkElementAsPlanned entity) {
            return entity.ElementName;
        }
        public static string toFullDescription(this Networkelementsasplanned entity, IRepositoryWrapper _repositoryWrapper)
        {
            var oem =  _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(x => x.Orgeqpmanufacturerid == entity.Orgeqpmanufacturerid).FirstOrDefault();

            return entity.Opco.Opco + " - " + oem.Originalequipmentmanufacturer + " - " + entity.Elementname;
        }
    }
}
