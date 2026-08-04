using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class LocationMapper
    {
        public static Models.Lookup.Location GetLocationMapper(Locations Location)
        {
            if (Location == null)
                return null;
            var model= new Models.Lookup.Location()
            {
                LocationId= Location.Locationid,
                LocationDescription = Location.Location,
                DefaultValue =Location.Defaultvalue,
                Locationdeploymenttypes = Location.Locationdeploymenttypes.Select(p=> LocationDeploymentTypeMapper.Get(p)).ToList(),
                OpcoId =Location.Opcoid,
                OpCo = OpCoMapper.GetOpCoMapper(Location.Opco),
                CreationDate = Location.Creationdate,
                CreationUser = Location.Creationuser,
                ModificationDate = Location.Modificationdate,
                ModificationUser = Location.Modificationuser,
                Deleted = Location.Deleted.Value,
                DeletionDate = Location.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(Location.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(Location.ModificationuserNavigation),
                ShortDescription = Location.Shortdescription
               
            };
            model.Locationdeploymenttypes = Location.Locationdeploymenttypes.Select(p => new LocationDeploymentTypes()
            {
                Id = p.Id,
                LocationId = p.Locationid.Value,
                DeploymentTypeId = p.Deploymenttypeid.Value,
                DeploymentType = DeploymentTypeMapper.GetDeploymentTypeMapper(p.Deploymenttype),

            }).ToList();
            return model;

        }
        public static Locations SetLocationMapper(Models.Lookup.Location Location)
        {
            var model=  new Locations()
            {

                Locationid = Location.LocationId,
                Location = Location.LocationDescription,
                Creationdate = Location.CreationDate,
                Creationuser = Location.CreationUser,
                Modificationdate = Location.ModificationDate,
                Modificationuser = Location.ModificationUser,
                Deleted = Location.Deleted,
                Deletiondate = Location.DeletionDate,
                Defaultvalue = Location.DefaultValue,
                //Locationtypeid = Location.Locationdeploymenttypes,
                Opcoid = Location.OpcoId,
                Shortdescription = Location.ShortDescription
            };
            model.Locationdeploymenttypes = Location.Locationdeploymenttypes.Select(p => new Locationdeploymenttypes()
            {
                Locationid = p.LocationId,
                Deploymenttypeid = p.DeploymentTypeId,
                Id = p.Id,
            }).ToList();

            return model;
        }
    }
}
