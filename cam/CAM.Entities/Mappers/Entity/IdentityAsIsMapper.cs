using CAM.Entities.Mappers.Cross;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using OracleModels.DBModels;
using System;

namespace CAM.Entities.Mappers
{
    public static class IdentityAsIsMapper
    {
        public static Models.IdentityAsIs Get(Identitiesasis model)
        {
            if (model == null)
                return null;
            var result= new Models.IdentityAsIs()
            {
                Id = model.Id,
                Value = model.Value,
                ClassId = model.Classid,
                CategoryId = model.Categoryid,
                AssetId = model.Assetid,
                PreviousResourceKey = model.Previousresourcekey,
                ResourceKey = model.Resourcekey,
                TypeId  = model.Typeid,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                Category = CategoryMapper.Get(model.Category),
                Class = ClassMapper.Get(model.Class),
                Type = TypeMapper.Get(model.Type),
                Asset = NetworkElementAsPlannedMapper.Get(model.Asset),
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                InterfaceType = model.Interfacetype,
                InterfaceName = model.Interfacename,

            };

            if (model.Asset.Networkelementasplannedsubdomainspoc != null)
            {
                foreach (var item in model.Asset.Networkelementasplannedsubdomainspoc)
                {
                    result.Asset.NetworkElementAsPlannedSubDomainSpoc.Add(NetworkElementAsPlannedSubDomainSpocMapper.GetSpocId(item));
                }
            }
            return result;
        }
        public static Identitiesasis Set(Models.IdentityAsIs model)
        {
            return new Identitiesasis()
            {
                Id = model.Id,
                Value = model.Value,
                Classid = model.ClassId,
                Categoryid = model.CategoryId,
                Assetid = model.AssetId,
                Previousresourcekey = model.PreviousResourceKey,
                Resourcekey = model.ResourceKey,
                Typeid = model.TypeId,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Interfacetype = model.InterfaceType,
                Interfacename = model.InterfaceName,
            };
        }
    }
}
