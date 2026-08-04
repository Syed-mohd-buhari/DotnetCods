using Oracle.ManagedDataAccess.Client;
using System;
using System.Security.AccessControl;
using System.Xml.Linq;
using TEMS.DataTransferObject;
using TEMS.Entity;
using static TEMS.Logs.Logger;
using TEMS.Logs;
using Microsoft.IdentityModel.Tokens;

namespace TEMS.Data
{
    public class ResourceKeyGeneration
    {
        private static Random random = new Random();
        public static string CreateorupdateIdentityResourceKey(OracleConnection connection, IdentityAsisCreateDto dto)
        {
            bool isError = false;
            var elementname =dto.ElementName;
            var opcoId = dto.OpCoId;
            var dcid = dto.AssetEntity["designcomponentid"];
            var dcfId = dto.AssetEntity["designcomponentfamilyid"];
            var buildBadid = dto.AssetEntity["buildbagid"];

            string resourceKeyQuery = $"Upper(OPCOID) = '{dto.OpCoId}' AND Upper(ELEMENTNAME) = '{dto.ElementName.ToUpper()}' AND Upper(RESOURCETYPESID) = '{4}' AND Upper(KEYSTATUS) = '{1}'";
            string resourceKeyExists = DataBaseFunctions.GetAttributesValue("resourcekeymaster", resourceKeyQuery);

 

            if (resourceKeyExists == "NA")
            {
                // generate Resourece key for identity

                string IdentityresourceKey = GenerateResourceKeyForIdentity(dto, dcfId);

                ResourceKeyMasterDto resourcekeyMasterDto = new ResourceKeyMasterDto();
                resourcekeyMasterDto.Dcfid = dcfId;
                resourcekeyMasterDto.Resourcetypesid = 4;
                resourcekeyMasterDto.Elementname = elementname;
                resourcekeyMasterDto.Opcoid = opcoId;
                resourcekeyMasterDto.Resourcekey = IdentityresourceKey;
                resourcekeyMasterDto.BuildBagId = buildBadid;

                var resourceKeyEntry = CreateOrUpdateResourceKey(connection, resourcekeyMasterDto, dto, dcfId).Result;
                if (resourceKeyEntry.Warning)
                {
                    return string.Empty;
                }
                else
                {
                    #region // DCFLC Entry code
                    var getDAndDcfNameDetailsQuery = CommonFunction.GetDCAndDCFNameRecords(dto.AssetEntity["networkelementasplannedid"]);
                    List<Dictionary<string, string>> dcAndDcfdetails = DataBaseFunctions.selectFromDBByQuery(getDAndDcfNameDetailsQuery);

                    foreach(var item in dcAndDcfdetails)
                    {
                        var opCoName = item["opco"];
                        var dcfName = GenerarteDCFName(item);
                        var dcName = GenerateDCName(item);
                        var dcId = item["designcomponentid"];
                        var bagName = item["bagdescription"];
                        var dcfLifeCycle = new DcfLifeCycle()
                        {
                            EventId = 0,
                            EventName = "Start Of Life Cycle",
                            Currentdetails = elementname,
                            Opcoid = opcoId,
                            Dcfid = dcfId,
                            Resourcekey = IdentityresourceKey,
                            Categorytype = 4,
                            Dcfdescription = dcfName,
                            Opcodescription = opCoName,
                            Dcdescription = dcName,
                            DcId = dcId,
                            BagName = bagName,

                        };


                        var dcfLifeCyclequery = GenerateDCFLifeCycleQuery(dcfLifeCycle);
                        Logger.WriteLog(TEMLog.Debug, "CreateorupdateIdentityResourceKey", $"{dcfLifeCyclequery}");
                        var pkid = DataBaseFunctions.DBInsertion(connection, dcfLifeCyclequery).ToString();
                        isError = pkid != null?false : true;
                        if (isError)
                        {
                            return string.Empty;
                        }
                    };

                    #endregion

                }


                return resourcekeyMasterDto.Resourcekey;
            }
            else
            {           
               return resourceKeyExists;
            }

        }

        public static string GenerateResourceKeyForIdentity(IdentityAsisCreateDto dto, string dcfId)
        {
            var resourceTypeId = 4;
            string resourceKeyQuery = $"Upper(OPCOID) = '{dto.OpCoId}' AND Upper(ELEMENTNAME) = '{dto.ElementName.ToUpper()}' AND Upper(RESOURCETYPESID) = '{resourceTypeId}' AND Upper(KEYSTATUS) = '{1}' AND Upper(DCFID) = '{dcfId}'";
            string resourceKey = DataBaseFunctions.GetAttributesValue("resourcekeymaster", resourceKeyQuery);


            if (resourceKey == "NA")
            {
                resourceKey = GenerateRandomResourceKey(resourceTypeId);
            }
            return resourceKey;
        }

        public static string GenerateRandomResourceKey(int resourceId)
        {
            int length = 7;
            string randomString = "";
            const string possibleChars = "ABCDEF0123456789";
            do
            {
                randomString = resourceId.ToString() + new string(Enumerable.Repeat(possibleChars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
            } while (IsResourceKeyExist(randomString));
            return randomString;

        }

        public static bool IsResourceKeyExist(string randomValue)
        {
            var ResourceMasterQuery = $"Upper(RESOURCEKEY) = '{randomValue}'";
            var ResourceMaster = DataBaseFunctions.GetPrimaryKey("resourcekeymaster", ResourceMasterQuery);
            if (ResourceMaster != "NA")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetResourceKeyId(IdentityAsisCreateDto dto, int resourceTypeId, string dcfId)
        {
            string resourceKeyQuery = $"Upper(OPCOID) = '{dto.OpCoId}' AND Upper(ELEMENTNAME) = '{dto.ElementName.ToUpper()}' AND Upper(RESOURCETYPESID) = '{resourceTypeId}' AND Upper(KEYSTATUS) = '{1}' AND Upper(DCFID) = '{dcfId}'";
            string resourceKey = DataBaseFunctions.GetPrimaryKey("resourcekeymaster", resourceKeyQuery);
            return resourceKey;
        }
        public static async Task<ResultDto> CreateOrUpdateResourceKey(OracleConnection connection, ResourceKeyMasterDto resourceKey, IdentityAsisCreateDto identityDto, string dcfId)
        {
            string reskeyEntityId = string.Empty;
            try
            {

                var reskey = GetResourceKeyId(identityDto, 4, dcfId);

                if (reskey != "NA")
                {
                    var resourceKeyQuery = $"Upper(RESOURCEKEY) = '{resourceKey.Resourcekey.ToUpper()}' AND Upper(RESOURCETYPESID) = '{4}'";
                    var resKeyExists = DataBaseFunctions.GetPrimaryKey("resourcekeymaster", resourceKeyQuery);

                    if (resKeyExists == null)
                    {
                        ResourceKeyMasterDto newResourceKeyMaster = new ResourceKeyMasterDto();
                        newResourceKeyMaster.Dcfid = resourceKey.Dcfid;
                        newResourceKeyMaster.Opcoid = resourceKey.Opcoid;
                        newResourceKeyMaster.Resourcekey = resourceKey.Resourcekey;
                        newResourceKeyMaster.Resourcetypesid = resourceKey.Resourcetypesid;
                        newResourceKeyMaster.Elementname = resourceKey.Elementname;
                        newResourceKeyMaster.Keystatus = 1;
                        newResourceKeyMaster.BuildBagId = resourceKey.BuildBagId;


                        var resourceKeyInsertionQuery = GenerateResourceKeyQuery(newResourceKeyMaster, identityDto.User);
                        reskeyEntityId = DataBaseFunctions.DBInsertion(connection, resourceKeyInsertionQuery).ToString();

                    }
                    else
                    {
                        ResourceKeyMasterDto updateResourceKeyMaster = new ResourceKeyMasterDto();
                        updateResourceKeyMaster.Dcfid = resourceKey.Dcfid;
                        updateResourceKeyMaster.Opcoid = resourceKey.Opcoid;
                        updateResourceKeyMaster.Elementname = resourceKey.Elementname;
                        updateResourceKeyMaster.Keystatus = 1;
                        updateResourceKeyMaster.Resourcekey = resourceKey.Resourcekey;
                        updateResourceKeyMaster.Resourcetypesid = resourceKey.Resourcetypesid;

                        var resourceKeyUpdationQuery = GenerateResourceKeyUpdateQuery(updateResourceKeyMaster, reskey, "resourcekeymaster", identityDto.User);
                        var rowsUpdated = DataBaseFunctions.DBUpdation(connection, resourceKeyUpdationQuery);
                    }

                    return new ResultDto
                    {
                        Data = reskey,
                        Warning = false,
                    };

                }
                else
                {
                    var resourceKeyQuery = $"Upper(RESOURCEKEY) = '{resourceKey.Resourcekey.ToUpper()}'";
                    var resKeyExists = DataBaseFunctions.GetPrimaryKey("resourcekeymaster", resourceKeyQuery);

                    if (resKeyExists == "NA")
                    {
                        ResourceKeyMasterDto newResourceKeyMaster = new ResourceKeyMasterDto();
                        newResourceKeyMaster.Dcfid = dcfId;
                        newResourceKeyMaster.Opcoid = resourceKey.Opcoid;
                        newResourceKeyMaster.Resourcekey = resourceKey.Resourcekey;
                        newResourceKeyMaster.Resourcetypesid = 4;
                        newResourceKeyMaster.Elementname = resourceKey.Elementname;
                        newResourceKeyMaster.Keystatus = 1;
                        newResourceKeyMaster.BuildBagId = resourceKey.BuildBagId;
                        //newResourceKeyMaster.Lifecycleid = identityDto.LifeCycleId;
                        var resourceKeyInsertionQuery = GenerateResourceKeyQuery(newResourceKeyMaster, identityDto.User);
                        reskeyEntityId = DataBaseFunctions.DBInsertion(connection, resourceKeyInsertionQuery).ToString();
                    }
                    else
                    {
                    }
                    return new ResultDto
                    {
                        Data = reskeyEntityId,
                        Warning = false,
                    };
                }

            }
            catch (Exception ex)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ex.Message,

                };
            }
        }
        public static string GenerateResourceKeyQuery(ResourceKeyMasterDto keyMasterDto, int user)
        {
            return $"INSERT INTO RESOURCEKEYMASTER (RESOURCETYPESID,RESOURCEKEY,OPCOID,DCFID,KEYSTATUS,BUILDBAGID,CREATIONUSER,MODIFICATIONUSER,ELEMENTNAME)" +
                $" VALUES ('{keyMasterDto.Resourcetypesid}','{keyMasterDto.Resourcekey}','{keyMasterDto.Opcoid}','{keyMasterDto.Dcfid}','{keyMasterDto.Keystatus}','{keyMasterDto.BuildBagId}','{user}','{user}','{keyMasterDto.Elementname}') " +
                $"returning {DBModelFunctions.GetPrimaryKey("resourcekeymaster")} into :pkcreated";
        }
        public static string GenerateResourceKeyUpdateQuery(ResourceKeyMasterDto dto, string ResPrimaryKey, string tableName, int user)
        {
            return $"UPDATE {tableName} SET dcfid='{dto.Dcfid}', opcoid = '{dto.Opcoid}', elementname = '{dto.Elementname}', keystatus = '{dto.Keystatus}', " +
                $"resourcekey = '{dto.Resourcekey}', resourcetypesid = '{dto.Resourcetypesid}', modificationuser={user}, modificationdate=SYS_EXTRACT_UTC(systimestamp) " +
                $"where {DBModelFunctions.GetPrimaryKey(tableName)}='{ResPrimaryKey}'";
        }

        public static string GenerateDCFLifeCycleQuery(DcfLifeCycle dto)
        {
            var insertQuery = CommonFunction.GetInsertionQueryForDcfLifeCycle("dcflifecycle", dto);
            return insertQuery;
        }
        public static string GenerarteDCFName(Dictionary<string, string> Dcfdetails)
        {
            try
            {
                var productName = Dcfdetails["productname"];
                var oem = Dcfdetails["originalequipmentmanufacturer"];
                var ismain = Dcfdetails["ismain"];
                var deleted = Dcfdetails["deleted"];
                var platformid = Dcfdetails["platformid"];
                var platform = (ismain == "1" && deleted == "0" && !platformid.IsNullOrEmpty()) ? Dcfdetails["platform"] : "";
                var alias = Dcfdetails["alias"];
                var subnetwork = Dcfdetails["subnetworkname"];
                var subnetworkboundary = alias.IsNullOrEmpty() ? alias : subnetwork;


                var result = $"{oem} " +
                    $"{productName} " +
                    $"<b class=\"text-lowercase\" >on</b> " +
                    $"{platform} " +
                    $"<b class=\"text-lowercase\"> for </b> " +
                    subnetworkboundary;
                return result;
            }
            catch
            {
                throw new Exception();
            }
        }
        public static string GenerateDCName(Dictionary<string, string> Dcdetails)
        {
            try
            {
                var productName = Dcdetails["productname"];
                var oem = Dcdetails["originalequipmentmanufacturer"];
                var isMain = Dcdetails["ismain"];
                var deleted = Dcdetails["deleted"];
                var platformId = Dcdetails["platformid"];
                var platform = Dcdetails["platform"];
                var alias = Dcdetails["alias"];
                var subNetwork = Dcdetails["subnetworkname"];
                var subNetworkBoundary = "<b class=\"text-lowercase\"> for </b>" + (alias.IsNullOrEmpty() ? alias : subNetwork);
                var softwareVersion = Dcdetails["softwareversion"];
                var rule = Dcdetails["rule"];
                var hardwareSolution = Dcdetails["hardwaresolution"];
                var hardwareType = Dcdetails["hardwaretype"];

                var name = $"{oem} {productName} {softwareVersion}";

                if(isMain == "1" && deleted == "0")
                {
                    if(rule.IsNullOrEmpty() && rule == "3")
                    {
                        name += $"<b class=\"text-lowercase\"> on </b> {platform}";
                    }
                    else
                    {
                        name += $"<b class=\"text-lowercase\"> on </b> {hardwareSolution} {platform} {hardwareType}";
                    }
                }
                if(isMain == "0" && deleted == "0")
                {
                    name += $"<b class=\"text-lowercase\"> with </b> {hardwareSolution} {platform} {hardwareType}";

                }

                var result = $"{name} {subNetworkBoundary}";

                return result ;

            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
