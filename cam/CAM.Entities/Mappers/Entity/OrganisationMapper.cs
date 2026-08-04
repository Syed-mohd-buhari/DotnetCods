using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Identity;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Entity
{
    public class OrganisationMapper
    {
        public static OrganisationModel GetOrganisationMapper(Organisation model)
        {
            if (model == null)
                return null;
            var result = new OrganisationModel()
            {
                OrganisationId = model.Organisationid,
                MainOrganisationId = model.Mainorganisationid,  
                VerticalResponsibleId = model?.Verticalid,
                VerticalResponsibleName = model?.Vertical?.Verticalresponsible,
                PracticeId = model.Practiceid,               
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,             
                Practice = PracticeMapper.GetPracticeMapper(model.Practice),
                MainOrganisation = MainOrganisationMapper.GetMainOrganisationMapper(model.Mainorganisation),
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                VerticalResponsible = VerticalResponsibleMapper.GetVerticalResponsibleMapper(model.Vertical),

            };
            return result;
        }

        public static Organisation Set(OrganisationModel model)
        {
            return new Organisation()
            {
                Organisationid = model.OrganisationId,
                Mainorganisationid = model.MainOrganisationId,
                Practiceid = model.PracticeId,
                Verticalid = model.VerticalResponsibleId,
                //Subdomainresponsibleid = model.SubdomainResponsibleId,
                //Contactid = model.ContactId,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                
            };
        }

        public static ApplicationOrganisation GetApplicationionOrganisation(Organisation model, bool include = false)
        {
            if (model == null)
                return null;
            var result = new ApplicationOrganisation()
            {               
                MainOranisationId = model.Mainorganisationid,
                PracticeId = model.Practiceid,
                //ApplicationSubDomainRes = SubDomainResponsibleMapper.GetApplicationSubDomainResponsible(model.Subdomainresponsible),
                MainOrganisation = MainOrganisationMapper.GetApplicationMainOrganisationMapper(model.Mainorganisation),
                Practice = PracticeMapper.GetApplicationPracticeMapper(model.Practice),
                VerticalRes = VerticalResponsibleMapper.GetApplicationVerticalResponsible(model.Vertical)
            };
            return result;
        }
    }
}
