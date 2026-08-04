using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class NetworkConstructMapper
    {
        public static Models.Lookup.NetworkConstruct GetNetworkConstructMapper(Networkconstructs NetworkConstruct)
        {
            if (NetworkConstruct == null)
                return null;
            return new Models.Lookup.NetworkConstruct()
            {
                NetworkConstructsId= NetworkConstruct.Networkconstructsid,
                NetworkConstructDescription = NetworkConstruct.Networkconstruct,
                CreationDate = NetworkConstruct.Creationdate,
                CreationUser = NetworkConstruct.Creationuser,
                ModificationDate = NetworkConstruct.Modificationdate,
                ModificationUser = NetworkConstruct.Modificationuser,
                Deleted = NetworkConstruct.Deleted.Value,
                DeletionDate = NetworkConstruct.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(NetworkConstruct.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(NetworkConstruct.ModificationuserNavigation),
               
            };
        }
        public static Networkconstructs SetNetworkConstructMapper(Models.Lookup.NetworkConstruct NetworkConstruct)
        {
            return new Networkconstructs()
            {

                Networkconstructsid = NetworkConstruct.NetworkConstructsId,
                Networkconstruct = NetworkConstruct.NetworkConstructDescription,
                Creationdate = NetworkConstruct.CreationDate,
                Creationuser = NetworkConstruct.CreationUser,
                Modificationdate = NetworkConstruct.ModificationDate,
                Modificationuser = NetworkConstruct.ModificationUser,
                Deleted = NetworkConstruct.Deleted,
                Deletiondate = NetworkConstruct.DeletionDate,
            };
        }
    }
}
