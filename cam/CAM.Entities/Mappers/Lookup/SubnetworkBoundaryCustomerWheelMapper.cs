using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public class SubnetworkBoundaryCustomerWheelMapper
    {
        public static SubnetworkBoundaryCustomerWheel Get(Subnetworkboundarycustomerwheel model)
        {
            if (model == null)
                return null;
            return new SubnetworkBoundaryCustomerWheel()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted != null ? model.Deleted.Value : false,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                SubnetworkBoundaryId = model.Subnetworkboundaryid.Value,
                Id = model.Id,
                CustomerWheelId = model.Customerwheelid.Value,
                CustomerWheel = CustomerWheelMapper.Get(model.Customerwheel),
            };
        }
        public static Subnetworkboundarycustomerwheel Set(SubnetworkBoundaryCustomerWheel model)
        {
            return new Subnetworkboundarycustomerwheel()
            {

                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Subnetworkboundaryid = model.SubnetworkBoundaryId,
                Id = model.Id,
                Customerwheelid = model.CustomerWheelId,
                Subnetworkboundary = SubNetworkBoundaryMapper.SetSubNetworkBoundaryMapper(model.SubNetworkBoundary),
                Customerwheel = CustomerWheelMapper.Set(model.CustomerWheel)



            };
        }
    }
}
