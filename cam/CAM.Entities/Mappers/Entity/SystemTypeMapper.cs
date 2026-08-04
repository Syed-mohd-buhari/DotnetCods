using CAM.Entities.Mappers.Cross;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System;
using System.Globalization;
using System.Linq;

namespace CAM.Entities.Mappers.Entity
{
    public static class SystemTypeMapper
    {
       
        public static SystemType GetSystemTypeMapper(Systemtypes model , bool Include =true)
        {
            if (model == null)
                return null;
            var result = new SystemType()
            {
                SystemTypeId = model.Systemtypeid,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                EndOfMaintenance = !model.Endofmaintenance.HasValue ? string.Empty : model.Endofmaintenance.Value.ToLongDateString(),
                EndOfMaintenanceValue =   model?.Endofmaintenance ,           
                SpareFieldsJson = model.Sparefieldsjson,
                AssetCategoryId = model.Assetcategoryid,
                AssetClassId = model.Assetclassid,
                AssetTypeId = model.Assettypeid,
                ConstraintLcm = model.Constraintlcm,
                ConstraintScaling = model.Constraintscaling,
                MajorSoftwareBuildsId = model.Majorsoftwarebuildsid,
                ProductImportanceId = model.Productimportanceid,
                //SubDomainResponsibleId = model.Subdomainresponsibleid,
                //SubDomainSpoc = model.Subdomainspoc,
                SystemTypeName3Gpp = model.Systemtypename3gpp,
                SystemTypeNameOem = model.Systemtypenameoem,
                
                VodafoneNameId = model.Vodafonename,
                VodafoneName = model.VodafonenameNavigation == null ? null : VodafoneNameMapper.GetVodafoneNamesMapper(model.VodafonenameNavigation),
                //VerticalResponsibleId = model.Verticalresponsibleid,
                AssetCategory = Include ? AssetCategoryMapper.GetAssetCategoryMapper(model.Assetcategory) : null,
                AssetClassIdNavigation = Include ? AssetClassMapper.GetAssetClassMapper(model.Assetclass) : null,
                AssetTypeIdNavigation = Include ? AssetTypeMapper.GetAssetTypeMapper(model.Assettype) :  null,
                MajorSoftwareBuilds = MajorSoftwareBuildMapper.GetMajorSoftwareBuildMapper(model.Majorsoftwarebuilds,false),
                ProductImportanceRel= Include ? ProductImportanceMapper.GetProductImportanceMapper(model.Productimportance) : null,
                //SubDomainResponsible = Include ? SubDomainResponsibleMapper.GetSubDomainResponsibleMapper(model.Subdomainresponsible):null,
                //VerticalResponsible = VerticalResponsibleMapper.GetVerticalResponsibleMapper(model.Verticalresponsible) ,
                DesignComponents = Include ? model.Designcomponents.Select(p=> DesignComponentMapper.GetDesignComponentMapper(p)).ToList() : null,

                //softWareDesignContactId

                //SoftWareDesignContactEmail = string.Join(",", model?.Majorsoftwarebuilds?.Majorswbuildsdesigncontacts.SelectMany(x => x?.Designcontact?.OrganisationContact.Select(x => x?.Contact?.Email)).ToList()),
                //HardWareDesignContactEmail = string.Join(",", model?.Systemtypesmajorhardwarebuilds.Select(x => x?.Majorhardware?.Majorhwbuildsdesigncontacts).FirstOrDefault().SelectMany(x => x?.Designcontact?.OrganisationContact
                //.Select(x => x?.Contact?.Email)).ToList()),

                //SoftWareVertical = string.Join(",", model?.Majorsoftwarebuilds?.Majorswbuildsdesigncontacts.SelectMany(x =>
                //x?.Designcontact?.OrganisationContact.Select(y => y?.Contact?.Aspnetuserroles.Select(z => z?.Verticalresponsible?.Verticalresponsible)))),


                //HardWareVertical = string.Join(",", model?.Systemtypesmajorhardwarebuilds.Select(x => x?.Majorhardware?.Majorhwbuildsdesigncontacts).FirstOrDefault().SelectMany(x =>
                // x?.Designcontact?.OrganisationContact.Select(y => y.Contact?.Aspnetuserroles.Select(z => z?.Verticalresponsible?.Verticalresponsible)))),



                //SoftWareSubdomain = string.Join(",", model?.Majorsoftwarebuilds?.Majorswbuildsdesigncontacts.SelectMany(x =>
                //x?.Designcontact?.OrganisationContact.Select(y => y?.Subdomainresponsible?.Subdomainresponsible))),


                //HardWareSubdomain = string.Join(",", model?.Systemtypesmajorhardwarebuilds?.Select(x => x.Majorhardware?.Majorhwbuildsdesigncontacts).FirstOrDefault().SelectMany(x =>
                //x?.Designcontact?.OrganisationContact.Select(y => y?.Subdomainresponsible?.Subdomainresponsible))),

            };
            if (model.Systemtypesmajorhardwarebuilds != null)
            {
                foreach (var item in model.Systemtypesmajorhardwarebuilds)
                {
                    result.SystemTypesMajorHardwareBuilds.Add(SystemTypesMajorHardwareBuildMapper.GetSystemTypesMajorHardwareBuildMapper(item));
                }
            }
            //if (model.Systemtypessubdomainspoc != null)
            //{
            //    foreach (var item in model.Systemtypessubdomainspoc)
            //    {
            //        result.SystemTypesSubDomainSpocs.Add(SystemTypesSubDomainSpocMapper.GetSystemTypesSubDomainSpocMapper(item));
            //    }
            //}
            return result;
        }

        public static SystemType GetSystemTypeGridMapper(Systemtypes model, bool Include = true)
        {
            if (model == null)
                return null;
            var result = new SystemType()
            {
                SystemTypeId = model.Systemtypeid,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                EndOfMaintenance = !model.Endofmaintenance.HasValue ? string.Empty : model.Endofmaintenance.Value.ToLongDateString(),
                EndOfMaintenanceValue = model?.Endofmaintenance,
                SpareFieldsJson = model.Sparefieldsjson,
                AssetCategoryId = model.Assetcategoryid,
                AssetClassId = model.Assetclassid,
                AssetTypeId = model.Assettypeid,
                ConstraintLcm = model.Constraintlcm,
                ConstraintScaling = model.Constraintscaling,
                MajorSoftwareBuildsId = model.Majorsoftwarebuildsid,
                ProductImportanceId = model.Productimportanceid,
                //SubDomainResponsibleId = model.Subdomainresponsibleid,
                //SubDomainSpoc = model.Subdomainspoc,
                SystemTypeName3Gpp = model.Systemtypename3gpp,
                SystemTypeNameOem = model.Systemtypenameoem,

                VodafoneNameId = model.Vodafonename,
                VodafoneName = model.VodafonenameNavigation == null ? null : VodafoneNameMapper.GetVodafoneNamesMapper(model.VodafonenameNavigation),
                //VerticalResponsibleId = model.Verticalresponsibleid,
                AssetCategory = Include ? AssetCategoryMapper.GetAssetCategoryMapper(model.Assetcategory) : null,
                AssetClassIdNavigation = Include ? AssetClassMapper.GetAssetClassMapper(model.Assetclass) : null,
                AssetTypeIdNavigation = Include ? AssetTypeMapper.GetAssetTypeMapper(model.Assettype) : null,
                MajorSoftwareBuilds = MajorSoftwareBuildMapper.GetMajorSoftwareBuildMapper(model.Majorsoftwarebuilds, false),
                ProductImportanceRel = Include ? ProductImportanceMapper.GetProductImportanceMapper(model.Productimportance) : null,
                //SubDomainResponsible = Include ? SubDomainResponsibleMapper.GetSubDomainResponsibleMapper(model.Subdomainresponsible):null,
                //VerticalResponsible = VerticalResponsibleMapper.GetVerticalResponsibleMapper(model.Verticalresponsible) ,
                DesignComponents = Include ? model.Designcomponents.Select(p => DesignComponentMapper.GetDesignComponentMapper(p)).ToList() : null,

                //softWareDesignContactId

                //SoftWareDesignContactEmail = string.Join(",", model?.Majorsoftwarebuilds?.Majorswbuildsdesigncontacts.SelectMany(x => x?.Designcontact?.OrganisationContact.Select(x => x?.Contact?.Email).Distinct()).ToList()),
                //HardWareDesignContactEmail = string.Join(",", model?.Systemtypesmajorhardwarebuilds.Select(x => x?.Majorhardware?.Majorhwbuildsdesigncontacts).FirstOrDefault().SelectMany(x => x?.Designcontact?.OrganisationContact
                //.Select(x => x?.Contact?.Email)).Distinct().ToList()),

                ////SoftWareVertical = string.Join(",", model?.Majorsoftwarebuilds?.Majorswbuildsdesigncontacts.SelectMany(x =>
                ////x?.Designcontact?.OrganisationContact.Select(y => y?.Contact?.Aspnetuserroles.Select(z => z?.Verticalresponsible?.Verticalresponsible))).Distinct().ToList()),


                //HardWareVertical = string.Join(",", model?.Systemtypesmajorhardwarebuilds.Select(x => x?.Majorhardware?.Majorhwbuildsdesigncontacts).FirstOrDefault().SelectMany(x =>
                // x?.Designcontact?.OrganisationContact.Select(y => y.Contact?.Aspnetuserroles.Select(z => z?.Verticalresponsible?.Verticalresponsible)))),



                //SoftWareSubdomain = string.Join(",", model?.Majorsoftwarebuilds?.Majorswbuildsdesigncontacts.SelectMany(x =>
                //x?.Designcontact?.OrganisationContact.Select(y => y?.Subdomainresponsible?.Subdomainresponsible)).Distinct().ToList()),


                //HardWareSubdomain = string.Join(",", model?.Systemtypesmajorhardwarebuilds?.Select(x => x.Majorhardware?.Majorhwbuildsdesigncontacts).FirstOrDefault().SelectMany(x =>
                //x?.Designcontact?.OrganisationContact.Select(y => y?.Subdomainresponsible?.Subdomainresponsible)).Distinct().ToList()),

            };
            if (model.Systemtypesmajorhardwarebuilds != null)
            {
                foreach (var item in model.Systemtypesmajorhardwarebuilds)
                {
                    result.SystemTypesMajorHardwareBuilds.Add(SystemTypesMajorHardwareBuildMapper.GetSystemTypesMajorHardwareBuildMapper(item));
                }
            }
            //if (model.Systemtypessubdomainspoc != null)
            //{
            //    foreach (var item in model.Systemtypessubdomainspoc)
            //    {
            //        result.SystemTypesSubDomainSpocs.Add(SystemTypesSubDomainSpocMapper.GetSystemTypesSubDomainSpocMapper(item));
            //    }
            //}
            return result;
        }
        public static Systemtypes SetSystemTypeMapper(SystemType model)
        {
            if (model == null)
                return null;
            DateTime? _EOMdate = System.DateTime.TryParse(model.EndOfMaintenance, out DateTime _endOfMaintenance) ? (DateTime?)_endOfMaintenance : null;
            var result = new Systemtypes()
            {
                Systemtypeid = model.SystemTypeId,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Endofmaintenance = _EOMdate,
                Sparefieldsjson = model.SpareFieldsJson,
                Assetcategoryid = model.AssetCategoryId,
                Assetclassid = model.AssetClassId,
                Assettypeid = model.AssetTypeId,
                Constraintlcm = model.ConstraintLcm,
                Constraintscaling= model.ConstraintScaling,
                Majorsoftwarebuildsid = model.MajorSoftwareBuildsId,
                Productimportanceid = model.ProductImportanceId,
                //Subdomainresponsibleid = model.SubDomainResponsibleId,
                //Subdomainspoc = model.SubDomainSpoc,
                Systemtypename3gpp = model.SystemTypeName3Gpp,
                Systemtypenameoem = model.SystemTypeNameOem,
                Vodafonename = model.VodafoneNameId,
                VodafonenameNavigation = VodafoneNameMapper.SetVodafoneNameMapper(model.VodafoneName),
                //Verticalresponsibleid = model.VerticalResponsibleId,
                Assetcategory = AssetCategoryMapper.SetAssetCategoryMapper(model.AssetCategory),
                Assetclass = AssetClassMapper.SetAssetClassMapper(model.AssetClassIdNavigation),
                Assettype = AssetTypeMapper.SetAssetTypeMapper(model.AssetTypeIdNavigation),
                Majorsoftwarebuilds = MajorSoftwareBuildMapper.SetMajorSoftwareBuildMapper(model.MajorSoftwareBuilds),
                Productimportance = ProductImportanceMapper.SetProductImportanceMapper(model.ProductImportanceRel),
                //Subdomainresponsible = SubDomainResponsibleMapper.SetSubDomainResponsibleMapper(model.SubDomainResponsible),
                //Verticalresponsible = VerticalResponsibleMapper.SetVerticalResponsibleMapper(model.VerticalResponsible),
            };
            if (model.SystemTypesMajorHardwareBuilds != null)
            {
                foreach (var item in model.SystemTypesMajorHardwareBuilds)
                {
                    item.SystemTypeId = model.SystemTypeId;
                    result.Systemtypesmajorhardwarebuilds.Add(SystemTypesMajorHardwareBuildMapper.SetSystemTypesMajorHardwareBuildMapper(item));
                }
            }
            if (model.SystemTypesSubDomainSpocs != null)
            {
                foreach (var item in model.SystemTypesSubDomainSpocs)
                {
                    item.SystemTypeId = model.SystemTypeId;
                    result.Systemtypessubdomainspoc.Add(SystemTypesSubDomainSpocMapper.SetSystemTypesSubDomainSpocMapper(item));
                }
            }
           
            return result;
        }



    }
}
