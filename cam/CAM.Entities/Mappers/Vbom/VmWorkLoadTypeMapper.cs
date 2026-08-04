using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.VBom;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Entities.Mappers.Vbom
{
    public class   VmWorkLoadTypeMapper
    {

        public static VmWorkLoadType GetVmWorkLoadType(Vmworkloadtype model)
        {
            if (model == null)
                return null;
            var result = new VmWorkLoadType()
            {
                VmworkLoadTypeId = model.Vmworkloadtypeid,
                Description = model.Description,
              
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
            };
             
            return result;
        }

        public static Vmworkloadtype SetVmWorkLoadType(VmWorkLoadType model)
        {
            if (model == null)
                return null;
            var result = new Vmworkloadtype()
            {
                Vmworkloadtypeid = model.VmworkLoadTypeId,
                Description = model.Description,
            };
            return result;
        }
    }
}
