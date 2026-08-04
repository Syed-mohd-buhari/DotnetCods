using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public class SubnetworkBoundarySystemFunctionMapper
    {
        public static SubnetworkBoundarySystemFunction Get(Subnetwrokboundarysystemfunction model) 
        {
            if (model == null)
                return null;
            return new SubnetworkBoundarySystemFunction()
            {
                CreationDate = model.Creationdate.Value,
                CreationUser = model.Creationuser.Value,
                ModificationDate = model.Modificationdate.Value,
                ModificationUser = model.Modificationuser.Value,
                Deleted = model.Deleted !=null ? model.Deleted.Value : false,
                DeletionDate =  model.Deleteddate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                SubnetworkBoundaryId = model.Subnetwrokboundaryid,
                Id = model.Id,
                SystemFunctionId = model.Systemfunctionid.Value,
                SystemFunction = SystemFunctionMapper.Get(model.Systemfunction),                
            };
        }
        public static Subnetwrokboundarysystemfunction Set(SubnetworkBoundarySystemFunction model)
        {
            return new Subnetwrokboundarysystemfunction()
            {
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deleteddate = model.DeletionDate,
                Subnetwrokboundaryid = model.SubnetworkBoundaryId,
                Id = model.Id,
                Systemfunctionid = model.SystemFunctionId,
                Subnetwrokboundary = SubNetworkBoundaryMapper.SetSubNetworkBoundaryMapper(model.SubNetworkBoundary),
                Systemfunction = SystemFunctionMapper.Set(model.SystemFunction),
            };
        }
    }
}
