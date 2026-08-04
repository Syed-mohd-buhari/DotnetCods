using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Mappers.Vbom;
using CAM.Entities.Models.AssetHardwareConfig;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Entities.Mappers.AssetHardwareConfig
{
    public class AssetAncillaryMapper
    {

        public static AssetHardwareAncillary GetAssetHardwareAncillary(Assethardwareancillary model)
        {
            if (model == null)
                return null;
            var result = new AssetHardwareAncillary()
            {
                AssetHardwareAncillaryId = model.Assethardwareancillaryid,
                MajorHardwareBuildAsIsId = model?.Majorhardwarebuildasisid,
                ClusterNameId = model?.Clusternameid,
                AssetClusterTypeId = model?.Assetclustertypeid,
                AssetClusterId = model?.Assetclusterid,
                NetworkElementAsPlannedId = model.Networkelementasplannedid,
                DataCenterId = model?.Datacenterid,

                ClusterNameDesc = model?.Clustername?.Clusterdescription,
                AssetClusterDesc = model?.Assetcluster?.Description,
                AssetClusterTypeDesc = model?.Assetclustertype?.Description,
                DataCeterName = model?.Datacenter?.Description,

                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
            };

            if (model.Assetcapacityinfo != null && model.Assetcapacityinfo.Count > 0)
            {
                Dictionary<string, short> vendor = new Dictionary<string, short>();
                if (model?.Majorhardwarebuildasis?.Orgeqpmanufacturer != null) {
                    vendor.Add(model?.Majorhardwarebuildasis?.Orgeqpmanufacturer.Originalequipmentmanufacturer,
                       (short) model?.Majorhardwarebuildasis?.Orgeqpmanufacturer.Orgeqpmanufacturerid);
                }

                Dictionary<string, short> hwModel = new Dictionary<string, short>();
                if (model?.Majorhardwarebuildasis?.Platform != null)
                {
                    hwModel.Add(model?.Majorhardwarebuildasis?.Platform.Platform,
                       (short)model?.Majorhardwarebuildasis?.Platform.Platformid);
                }


                foreach (var item in model.Assetcapacityinfo)
                {
                    result.AssetCapacityInfo.Add(AssetCapacityInfoMapper.GetAssetCapacityInfo(item, hwModel, vendor));
                }
            }
            return result;
        }

        public static Assethardwareancillary SetAssetHardwareAncillary(AssetHardwareAncillary model)
        {
            if (model == null)
                return null;
            var result = new Assethardwareancillary()
            {
                Assethardwareancillaryid = model.AssetHardwareAncillaryId,
                Majorhardwarebuildasisid = model?.MajorHardwareBuildAsIsId,
                Clusternameid = model?.ClusterNameId,
                Assetclusterid = model?.AssetClusterId,
                Assetclustertypeid  = model?.AssetClusterTypeId
            };
            return result;
        }
    }
}
