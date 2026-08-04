using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System;

namespace CAM.Entities.Mappers.Entity
{
    public class HardwareConfigurationMapper
    {
        public static HardwareConfiguration Get(Hardwareconfiguration model)
        {
            if (model == null)
                return null;
            return new HardwareConfiguration()
            {
                Hardwareconfigurationid = model.Hardwareconfigurationid,
                Networkelementid = model.Networkelementid,
                Oem =model.Oem,
                Opco = model.Opco,
                Elementname = model.Elementname,
                Hardwaretype = model.Hardwaretype,
                Productname = model.Productname,
                Serialnumber = model.Serialnumber,
                Unitlocation = model.Unitlocation,
                Vendor = model.Vendor,
                Productnumber=model.Productnumber,
                Revision=model.Revision,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation)

            };
        }

        public static Hardwareconfiguration Set(HardwareConfiguration model)
        {
            return new Hardwareconfiguration()
            {
                //Hardwareconfigurationid = model.Hardwareconfigurationid,
                //Networkelementid = model.Networkelementid,
                //Opco = model.Opco,
                //Oem = model.Oem,
                //Elementname = model.Elementname,
                //Productname = model.Productname,
                //Productno = model.Productno,
                //Productrevision = model.Productrevision,
                //Serialnumber = model.Serialnumber,
                //Unitlocation = model.Unitlocation,
                //Vendor = model.Vendor,
                //Creationdate = model.Creationdate,
                //Creationuser = model.Creationuser,
                //Modificationdate = model.Modificationdate,
                //Modificationuser = model.Modificationuser

            };
        }
    }
}
