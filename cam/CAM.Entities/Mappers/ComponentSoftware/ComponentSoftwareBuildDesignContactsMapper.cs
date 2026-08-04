using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Lookup
{
    public static class ComponentSoftwareBuildDesignContactsMapper
    {
        public static ComponentSoftwareBuildsDesignContact GetComponentSWDesignContact(Componentsoftwarebuildsdesigncontacts model)
        {
            if (model == null)
                return null;
            return new ComponentSoftwareBuildsDesignContact()
            {
                ComponentSoftwareBuildId = model.Componentsoftwarebuildid,
                ComponentSoftwarebuildsDesignContactId = model.Componentsoftwarebuildsdesigncontactid,
                DesignContactId = model.Designcontactid,
                DesignContact = ApplicationUserMapper.GetApplicationUserMapper(model.Designcontact),    
            };
        }

        public static Componentsoftwarebuildsdesigncontacts SetComponentSWDesignContact(ComponentSoftwareBuildsDesignContact model)
        {
            if (model == null)
                return null;
            return new Componentsoftwarebuildsdesigncontacts()
            {
                Componentsoftwarebuildid = model.ComponentSoftwareBuildId,
                Componentsoftwarebuildsdesigncontactid = model.ComponentSoftwarebuildsDesignContactId,
            
            };
        }
    }
}
