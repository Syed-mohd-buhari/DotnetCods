using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Entities.Mappers.Entity
{
    public class ResourceKeyMasterMapper
    {
        public static ResourceKeyMaster Get(Resourcekeymaster model)
        {
            if (model == null)
                return null;
            return new ResourceKeyMaster()
            {
                ResourceKeyMasterId = model.Resourcekeymasterid,
                ResourceTypesId = model.Resourcetypesid,
                ResourceKey = model.Resourcekey,
                OpcoId = model.Opcoid,
                DcfId = model.Dcfid,
                ElementName=model.Elementname,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                Opco = OpCoMapper.GetOpCoMapper(model.Opco),
                KeyStatus= model.Keystatus,
                ResourceTypes = ResourceTypeMapper.GetResourcetypes(model.Resourcetypes),
                DesignComponentFamily=DesignComponentFamilyMapper.Get(model.Dcf,false),
                LifeCycleId = model.Lifecycleid ,
                BuildBagId = model.Buildbagid,               
            };
        }

        public static Resourcekeymaster Set(ResourceKeyMaster model)
        {
            return new Resourcekeymaster() {
                Resourcekeymasterid = model.ResourceKeyMasterId,
                Resourcetypesid = model.ResourceTypesId,
                Resourcekey = model.ResourceKey,
                Opcoid = model.OpcoId,
                Dcfid = model.DcfId,
                Elementname=model.ElementName,
                Keystatus = model.KeyStatus,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Lifecycleid = model.LifeCycleId,
                Buildbagid = model.BuildBagId
            };
        }
    }
}
