using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System.Collections.Generic;
using System.Linq;

namespace CAM.Entities.Mappers.Cross
{
    public static class BuildBagMapper
    {

        public static BuildBag GetBuildBag(Buildbags model)
        {

            if (model == null)
                return null;
            var buildBagItem = new BuildBag()
            {
                CreationUser = model.Creationuser,
                CreationDate = model.Creationdate,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationuserNavigation = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationuserNavigation = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                OpCoId = model.Opcoid,
                DesignComponentFamilyId = model.Designcomponentfamilyid,
                OpCo = OpCoMapper.GetOpCoMapper(model.Opco),
                DesignComponentFamily = DesignComponentFamilyMapper.Get(model.Designcomponentfamily),
                BuildBagId = model.Buildbagid,
                BagDescription = model.Bagdescription,
                BagVersion = model.Bagversion,
                VisibleFlag=model.Visibleflag??false,
                MappingComponetSoftwareDescription = ((model.Componentsoftwarebuildbags.Any()) ?
               model.Componentsoftwarebuildbags.Select(s => new FilterValueDtoKeyValueList
               {
                   Key = (int)s.Componentsoftwarebuildbagid,
                   Value = $"{s.Componentsoftwarebuild.Componentmanufacturer.Componentmanufacturer}-" +
                                         $"{s.Componentsoftwarebuild.Componentmanufacturer.Componentname}-" +
                                         $"{s.Componentsoftwarebuild.Softwareversion}\n"
               }).ToList()

               : new List<FilterValueDtoKeyValueList>())
            };


            if (model.Lcmengineering != null)
            {
                var lcmEntity = model.Lcmengineering.Where(t => t.Deleted == false && t.Archived == false).FirstOrDefault();

                buildBagItem.LcmEngineering = lcmEntity != null
                          ? new List<LcmEngineering> { new LcmEngineering { LcmengineeringId = lcmEntity.Lcmengineeringid } }
                          : new List<LcmEngineering>();



            }
            return buildBagItem;
        }

        public static BuildBag GetBuildBagDropdownAsync(Buildbags model)
        {

            if (model == null)
                return null;
            var buildBagItem = new BuildBag()
            {
                
                OpCoId = model.Opcoid,
                DesignComponentFamilyId = model.Designcomponentfamilyid,                
                BuildBagId = model.Buildbagid,
                BagDescription = model.Bagdescription,
                BagVersion = model.Bagversion,              
            };
             
            return buildBagItem;
        }

        public static Buildbags SetBuildBag(BuildBag model)
        {
            return new Buildbags()
            {

                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Buildbagid = model.BuildBagId,
                Bagdescription = model.BagDescription,
                Bagversion = model.BagVersion,
                Opcoid = model.OpCoId,
                Designcomponentfamilyid = model.DesignComponentFamilyId,
                Visibleflag=model.VisibleFlag,
            };
        }
    }
}
