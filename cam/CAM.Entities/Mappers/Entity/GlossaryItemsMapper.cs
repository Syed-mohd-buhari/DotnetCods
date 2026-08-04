using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Entity
{
    public static class GlossaryItemsMapper
    {
        public static GlossaryItems GetGlossaryItemMapper(Glossaryitems model, bool Include = true)
        {
            if (model == null)
                return null;
            var result = new GlossaryItems()
            {
                GlossaryItemsId = model.Glossaryitemsid,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                Description = model.Description,
                Header = model.Header,
                Istsrfield = model.Istsrfield,
            };
            return result;
        }

        public static Glossaryitems SetGlossaryItemMapper(GlossaryItems model)
        {
            if (model == null)
                return null;
            var result = new Glossaryitems()
            {
                Glossaryitemsid = model.GlossaryItemsId,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Description = model.Description,
                Header = model.Header,
                Istsrfield = model.Istsrfield,
            };
            return result;
        }
    }
}
