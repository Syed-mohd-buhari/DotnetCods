using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.OMC;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Cbom
{
    public class AssetAsIsHwAncillaryDataMapper
    {
        public static AssetAsIsHwAncillaryData Get(Assetasishwancillarydata model)
        {
            if (model == null)
                return null;
            var result = new AssetAsIsHwAncillaryData()
            {
                Assetasishwancillarydataid = model.Assetasishwancillarydataid,
                Networkelementasisid = model.Networkelementasisid,
                Site = model.Site,
                Host = model.Host,
                Datasourcename = model.Datasourcename,
                Provider = model.Provider,
                Providertype = model.Providertype,
                Consumer = model.Consumer,
                Consumertype = model.Consumertype,
                Consumerrole = model.Consumerrole,
                Clustername = model.Clustername,
                Partnumber = model.Partnumber,
                Systemtype = model.Systemtype,
                Manufacturer = model.Manufacturer,
                Biosversion = model.Biosversion,
                Model = model.Model,
                Sku = model.Sku,
                Cpucapacity = model.Cpucapacity,
                Ephemeralstoragecapacity = model.Ephemeralstoragecapacity,
                Memorycapacity = model.Memorycapacity,
                Processorsummarymodel = model.Processorsummarymodel,
                Kubernetesnodetype = model.Kubernetesnodetype,
                Managementip = model.Managementip,
                Kubernetesnodename = model.Kubernetesnodename,
                Kubeletversion = model.Kubeletversion,
                Kubernetesnodeos = model.Kubernetesnodeos,
                Kubernetesnoderesourcetype = model.Kubernetesnoderesourcetype,
                Kubernetesnodestate = model.Kubernetesnodestate,
                Kubernetesnodestatusupdatetime = model.Kubernetesnodestatusupdatetime,
                Chassisdetails = model.Chassisdetails,
                DeletionDate = model.Deletiondate,
                Deleted = model.Deleted.Value,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
            };

            return result;
        }

        public static Assetasishwancillarydata Set(AssetAsIsHwAncillaryData model)
        {
            if (model == null)
                return null;
            var result = new Assetasishwancillarydata()
            {
                Assetasishwancillarydataid = model.Assetasishwancillarydataid,
                Networkelementasisid = model.Networkelementasisid,
                Site = model.Site,
                Host = model.Host,
                Datasourcename = model.Datasourcename,
                Provider = model.Provider,
                Providertype = model.Providertype,
                Consumer = model.Consumer,
                Consumertype = model.Consumertype,
                Consumerrole = model.Consumerrole,
                Clustername = model.Clustername,
                Partnumber = model.Partnumber,
                Systemtype = model.Systemtype,
                Manufacturer = model.Manufacturer,
                Biosversion = model.Biosversion,
                Model = model.Model,
                Sku = model.Sku,
                Cpucapacity = model.Cpucapacity,
                Ephemeralstoragecapacity = model.Ephemeralstoragecapacity,
                Memorycapacity = model.Memorycapacity,
                Processorsummarymodel = model.Processorsummarymodel,
                Kubernetesnodetype = model.Kubernetesnodetype,
                Managementip = model.Managementip,
                Kubernetesnodename = model.Kubernetesnodename,
                Kubeletversion = model.Kubeletversion,
                Kubernetesnodeos = model.Kubernetesnodeos,
                Kubernetesnoderesourcetype = model.Kubernetesnoderesourcetype,
                Kubernetesnodestate = model.Kubernetesnodestate,
                Kubernetesnodestatusupdatetime = model.Kubernetesnodestatusupdatetime,
                Chassisdetails = model.Chassisdetails,
                Deletiondate = model.DeletionDate,
                Deleted = model.Deleted,

            };
            return result;
        }
    }
}
