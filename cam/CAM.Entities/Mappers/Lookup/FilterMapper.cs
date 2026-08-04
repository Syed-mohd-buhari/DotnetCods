using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Entities.Models.Lookup;
using Microsoft.AspNetCore.SignalR;
using OracleModels.DBModels;
using System;
using System.Linq;

namespace CAM.Entities.Mappers.Lookup
{
    public class FilterMapper
    {
        public static SoftwareConfigurationFamily Get(Subfunctionarea model)
        {
            if (model == null)
                return null;
            var result = new SoftwareConfigurationFamily()
            {
                Opco =model.Subfunction.Functionarea.Function.Softwareconfiguration.Opco,
                oem = model.Subfunction.Functionarea.Function.Softwareconfiguration.Oem,
                ElementName = model.Subfunction.Functionarea.Function.Softwareconfiguration.Elementname,

            };
            return result;
        }

        //public static Component Set(Components model)
        //{
        //    return new Component()
        //    {
        //        Componentid = model.Componentid,
        //        Softwarecomponentid = model.softwarecomponentid,
        //        //ElementName = model.Elementname,
        //        //Opco = model.OpCO,
        //        //Oem = model.Oem,
        //        //NetworkElementId = model.NetworkElementId,
        //        //Mainsoftwareversion = model.Mainsoftwareversion,
        //        Componentname = model.Componentname,
        //        Productiondate = model.Productiondate,
        //        Productionnumber = model.Productionnumber,
        //        Productionrevision = model.ProductionRevision,
        //        //Creationdate = model.ComponentCreationdate,
        //        //Creationuser = model.ComponentCreationuser,
        //        //Modificationdate = model.ComponentModificationdate,
        //        //Modificationuser = model.ComponentModificationuser,
        //        Creationdate = model.Creationdate,
        //        Creationuser = model.Creationuser,
        //        Modificationdate = model.Modificationdate,
        //        Modificationuser = model.Modificationuser,
        //    };
       // }
    }
}
