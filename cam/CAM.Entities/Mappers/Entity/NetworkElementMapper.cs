using AutoMapper;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System;

namespace CAM.Entities.Mappers.Entity
{
    public class NetworkElementMapper
    {
        public static NetworkElement Get(Networkelement model)
        {
            if (model == null)
                return null;
            var result = new NetworkElement()
            {
                Networkelementid = model.Networkelementid,
                Opco = model.Opco,
                Oem = model.Oem,
                Nodetype = model.Nodetype,
                Elementname = model.Elementname,
                Dataacquisitiondate = model.Dataacquisitiondate,
                Platformtype = model.Platformtype,
                Sitelocation = model.Sitelocation,
                Softwareinstalldate = model.Softwareinstalldate,
                Softwareinstalldateap = model.Softwareinstalldateap,
                Softwareinstalldatecp = model.Softwareinstalldatecp,
                Softwareproductdate = model.Softwareproductdate,
                Softwareproductdateap = model.Softwareproductdateap,
                Softwareproductdatecp = model.Softwareproductdatecp,
                Softwareproductnumber = model.Softwareproductnumber,
                Softwareproductnumberap = model.Softwareproductnumberap,
                Softwareproductnumbercp = model.Softwareproductnumbercp,
                Softwarereleaseinformation = model.Softwarereleaseinformation,
                Softwarereleaseinformationap = model.Softwarereleaseinformationap,
                Softwarereleaseinformationcp = model.Softwarereleaseinformationcp,
                Spare1ossorenm = model.Spare1ossorenm,
                Spare2xmlversion = model.Spare2xmlversion,
                NodeTypeName = model.Nodetypename,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Xmllastparsefiledate = model.Xmllastparsefiledate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation)

            };
            if(model.Softwareconfiguration != null)
            {
                foreach(var item in model.Softwareconfiguration)
                {
                    result.SoftwareConfiguration.Add(SoftwareConfigurationMapper.GetSoftwareConfiguration(item));
                }
            }
            return result;
        }

        public static Networkelement Set(NetworkElement model)
        {
            if (model == null)
                return null;
            return new Networkelement()
            {

                Networkelementid = model.Networkelementid,
                Opco = model.Opco,
                Oem = model.Oem,
                Nodetype = model.Nodetype,
                Elementname = model.Elementname,
                Dataacquisitiondate = model.Dataacquisitiondate,
                Platformtype = model.Platformtype,
                Sitelocation = model.Sitelocation,
                Softwareinstalldate = model.Softwareinstalldate,
                Softwareinstalldateap = model.Softwareinstalldateap,
                Softwareinstalldatecp = model.Softwareinstalldatecp,
                Softwareproductdate = model.Softwareproductdate,
                Softwareproductdateap = model.Softwareproductdateap,
                Softwareproductdatecp = model.Softwareproductdatecp,
                Softwareproductnumber = model.Softwareproductnumber,
                Softwareproductnumberap = model.Softwareproductnumberap,
                Softwareproductnumbercp = model.Softwareproductnumbercp,
                Softwarereleaseinformation = model.Softwarereleaseinformation,
                Softwarereleaseinformationap = model.Softwarereleaseinformationap,
                Softwarereleaseinformationcp = model.Softwarereleaseinformationcp,
                Spare1ossorenm = model.Spare1ossorenm,
                Spare2xmlversion = model.Spare2xmlversion,
                Nodetypename=model.NodeTypeName,
                //Creationdate = model.Creationdate,
                //Creationuser = model.Creationuser,
                //Modificationdate = model.Modificationdate,
                //Modificationuser = model.Modificationuser,
                Xmllastparsefiledate = model.Xmllastparsefiledate,
            };
        }
    }
}
