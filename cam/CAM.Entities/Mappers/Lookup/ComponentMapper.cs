using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System;
using System.Linq;

namespace CAM.Entities.Mappers.Lookup
{
    public class ComponentMapper
    {
        public static Components Get(Component model)
        {
            if (model == null)
                return null;
            return new Components()
            {

                Elementname = model.Softwarecomponent.Elementname,
                OpCO = model.Softwarecomponent.Opco,
                Oem = model.Softwarecomponent.Oem,
                NetworkElementId = model.Softwarecomponent.Networkelementid,
                Mainsoftwareversion = model.Softwarecomponent.Mainsoftwareversion,
                Creationdate = model.Softwarecomponent.Creationdate,
                Creationuser = model.Softwarecomponent.Creationuser,
                Modificationdate = model.Softwarecomponent.Modificationdate,
                Modificationuser = model.Softwarecomponent.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                Componentid = model.Componentid,
                softwarecomponentid = model.Softwarecomponentid,
                Componentname = model.Componentname,
                Productiondate = model.Productiondate,
                Productionnumber = model.Productionnumber,
                ProductionRevision = model.Productionrevision,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
               

            };
        }

        public static Component Set(Components model)
        {
            return new Component()
            {
                Componentid = model.Componentid,
                Softwarecomponentid = model.softwarecomponentid,
                Componentname = model.Componentname,
                Productiondate = model.Productiondate,
                Productionnumber = model.Productionnumber,
                Productionrevision = model.ProductionRevision,
                Creationdate = model.Creationdate,
                Creationuser = model.Creationuser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser
            };
        }
    }
}
