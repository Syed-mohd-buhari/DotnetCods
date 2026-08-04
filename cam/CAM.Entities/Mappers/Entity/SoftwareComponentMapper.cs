using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System;
using System.Linq;

namespace CAM.Entities.Mappers.Entity
{
    public class SoftwareComponentMapper
    {
        public static SoftwareComponent Get(Softwarecomponent model)
        {
            if (model == null)
                return null;
            return new SoftwareComponent()
            {
                SoftwarecomponentId = model.Softwarecomponentid,
                NetworkelementId = model.Networkelementid,
                Oem = model.Oem,
                Opco = model.Opco,
                Elementname = model.Elementname,
                Mainsoftwareversion = model.Mainsoftwareversion,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser=model.Modificationuser,
                
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                //Component = model.Component,
                //xxx
                //Modificationuser = model.Modificationuser,
                //Componentname = model.Component.,
                //Productiondate = model.Productiondate,
                //Productionnumber = model.Productionnumber,
                //Productionrevision = model.Productionrevision,
                //Componentcreationdate = model.Componentcreationdate,
                //Componentcreationuser = model.Componentcreationuser,
                //Componentmodificationdate = model.Componentmodificationdate,
                //Componentmodificationuser = model.Componentmodificationuser,

            };
        }

        public static Softwarecomponent Set(SoftwareComponent model)
        {
            return new Softwarecomponent()
            {
                //xx
                //SoftwareComponentId = model.SoftwarecomponentId,
                //Networkelementid = model.NetworkelementId,
                //Oem = model.Oem,
                //Opco = model.Opco,
                //Elementname = model.Elementname,
                //Mainsoftwareversion = model.Mainsoftwareversion,
                //Creationdate = model.Creationdate,
                //Creationuser = model.Creationuser,
                //Modificationdate = model.Modificationdate,
                //Modificationuser = model.Modificationuser,
                //Componentname = model.Componentname,
                //Productiondate = model.Productiondate,
                //Productionnumber = model.Productionnumber,
                //Productionrevision = model.Productionrevision,
                //Componentcreationdate = model.Componentcreationdate,
                //Componentcreationuser = model.Componentcreationuser,
                //Componentmodificationdate = model.Componentmodificationdate,
                //Componentmodificationuser = model.Componentmodificationuser,

            };
        }
    }
}
