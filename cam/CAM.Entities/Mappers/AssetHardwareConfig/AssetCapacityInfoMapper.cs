using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.AssetHardwareConfig;
using OracleModels.DBModels;
using System.Collections.Generic;
using System.Linq;

namespace CAM.Entities.Mappers.AssetHardwareConfig
{
    public class AssetCapacityInfoMapper
    {

        public static AssetCapacityInfo GetAssetCapacityInfo(Assetcapacityinfo model, Dictionary<string, short> hwModel, Dictionary<string, short> vendor)
        {
            if (model == null)
                return null;
            var result = new AssetCapacityInfo()
            {
                AssetHardwareAncillaryId = model.Assethardwareancillaryid,
                AssetCapacityInfoId = model.Assetcapacityinfoid,
                PhysicalServerSerialNumber = model.Physicalserverserialnumber,
                PhysicalServerHostName = model.Physicalserverhostname,
                PhysicalServerIpAddress = model.Physicalserveripaddress,
                Memory = model.Memory,
                Storage = model.Storage,
                Vcpu = model.Vcpu,
                NoOfInstances = model.Noofinstances,             
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
            };
            if(hwModel != null && hwModel.Count >0)
            {
                result.PhysicalServerHwModel = hwModel.First().Key;
                    result.PhysicalServerHwModelId = hwModel.FirstOrDefault().Value;
            }
            if (vendor != null && vendor.Count > 0)
            {
                result.PhysicalServerVendor = vendor.First().Key;
                result.PhysicalServerVendorId = vendor.FirstOrDefault().Value;
            }
            return result;
        }

        public static Assetcapacityinfo SetAssetCapacityInfo(AssetCapacityInfo model)
        {
            if (model == null)
                return null;
            var result = new Assetcapacityinfo()
            {
                Assethardwareancillaryid = model.AssetHardwareAncillaryId,
                Assetcapacityinfoid = model.AssetCapacityInfoId,
                Physicalserverhostname = model?.PhysicalServerHostName,
                Physicalserveripaddress = model?.PhysicalServerIpAddress,
                Physicalserverserialnumber = model?.PhysicalServerSerialNumber,
                Memory = model?.Memory,
                Storage = model?.Storage,   
                Vcpu = model?.Vcpu,
                Noofinstances = model?.NoOfInstances
            };
            return result;
        }
    }
}
