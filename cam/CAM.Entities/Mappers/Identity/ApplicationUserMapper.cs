using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Mappers.RBAC;
using CAM.Identity;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAM.Entities.Mappers.Identity
{
    public static class ApplicationUserMapper
    {
        public static ApplicationUser GetApplicationUserMapper (Aspnetusers aspnetuser, bool include = false)
        {
            if (aspnetuser == null)
                return null;
            var result = new ApplicationUser()
            {
                   Id =aspnetuser.Id,
                   AccessFailedCount = aspnetuser.Accessfailedcount,
                   Email =aspnetuser.Email,
                   ConcurrencyStamp =aspnetuser.Concurrencystamp,
                   EmailConfirmed =aspnetuser.Emailconfirmed,
                   LockoutEnabled =aspnetuser.Lockoutenabled,
                   LockoutEnd =aspnetuser.Lockoutend,
                   NormalizedEmail =aspnetuser.Normalizedemail,
                   NormalizedUserName =aspnetuser.Normalizedusername,
                   PasswordHash =aspnetuser.Passwordhash,
                   PhoneNumber =aspnetuser.Phonenumber,
                   PhoneNumberConfirmed =aspnetuser.Phonenumberconfirmed,
                   SecurityStamp =aspnetuser.Securitystamp,
                   TwoFactorEnabled =aspnetuser.Twofactorenabled,
                   UserName =aspnetuser.Username,
                   Active =aspnetuser.Active,
                   Isdesigncontact = aspnetuser.Isdesigncontact,
                   Iseduspoc = aspnetuser.Iseduspoc,
                   Issubdomainspoc = aspnetuser.Issubdomainspoc,
                   Subdomainresponsibleid = aspnetuser.Subdomainresponsibleid,
                   ApplicationSubDomainRes = SubDomainResponsibleMapper.GetApplicationSubDomainResponsible(aspnetuser.Subdomainresponsible)
                                  
            };
            result.UserRoles = include == true ? aspnetuser.Aspnetuserroles.Select(x => AspnetuserroleMapper.GetUserApplication(x, false)).ToList() : null;
            result.ApplicationOrgAndVerticals = aspnetuser.AspnetuserverticalsUser != null && aspnetuser.AspnetuserverticalsUser.Count >0 ? aspnetuser.AspnetuserverticalsUser.Select(x => AspNetUserVerticalsMapper.GetOrgVerticalApplication(x)).ToList() 
                : new List<ApplicationOrgandVertical>();
            result .Opcos  = aspnetuser.AspnetuseropcosUser != null && aspnetuser.AspnetuseropcosUser.Count > 0 ? aspnetuser.AspnetuseropcosUser.Select(x => AspNetUserOpcosMapper.GetAspNetUserOpcoApplication(x)).ToList() :
                new List<ApplicationUserOpco> ();
            result.Modificationdate = aspnetuser.Modificationdate;
            result.Modificationuser= aspnetuser.Modificationuser;
            return result;
        }

        public static Aspnetusers SetApplicationUserMapper(ApplicationUser aspnetuser)
        {
            if (aspnetuser != null)
                return new Aspnetusers()
                {
                    Id = aspnetuser.Id,
                    Accessfailedcount = aspnetuser.AccessFailedCount,
                    Email = aspnetuser.Email,
                    Concurrencystamp = aspnetuser.ConcurrencyStamp,
                    Emailconfirmed = aspnetuser.EmailConfirmed,
                    Lockoutenabled = aspnetuser.LockoutEnabled,
                    Normalizedemail = aspnetuser.NormalizedEmail,
                    Normalizedusername = aspnetuser.NormalizedUserName,
                    Passwordhash = aspnetuser.PasswordHash,
                    Phonenumber = aspnetuser.PhoneNumber,
                    Phonenumberconfirmed = aspnetuser.PhoneNumberConfirmed,
                    Securitystamp = aspnetuser.SecurityStamp,
                    Twofactorenabled = aspnetuser.TwoFactorEnabled,
                    Username = aspnetuser.UserName,
                    Active = aspnetuser.Active,
                    Isdesigncontact = aspnetuser.Isdesigncontact,
                    Iseduspoc = aspnetuser.Iseduspoc,
                    Issubdomainspoc = aspnetuser.Issubdomainspoc,
                    Subdomainresponsibleid = aspnetuser.Subdomainresponsibleid,
                };
            return null;
        }
    }
}
