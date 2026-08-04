using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Entity
{
    public class ReconciliationMapper
    {
        public static ReconciliationModel Get(Reconciliations model)
        {
            if (model == null)
                return null;
            var result = new ReconciliationModel()
            {  
                ReconciliationId = model.Reconciliationid,           
                OpCo = model.Opco,                
                ElementName = model.Elementname, 
                DeploymentStatus = model.Deploymentstatus,
                CurrentSwVersion = model.Currentswversion,
                NewSwVersion = model.Newswversion,
                Status = model.Status,
                AssetId = model.Assetid,
                Creationdate = model.Creationdate,
                Creationuser = model.Creationuser,
                Modificationdate = model.Modificationdate,
                Modificationuser = model.Modificationuser,
                Deleted = model.Deleted,
                //CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                //ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),                              
            };
            return result;
        }

        public static Reconciliations Set(ReconciliationModel model)
        {
            return new Reconciliations()
            {

                Reconciliationid = model.ReconciliationId,
                Opco = model.OpCo,               
                Elementname = model.ElementName,
                Deploymentstatus = model.DeploymentStatus,
                Assetid = model.AssetId,
                Currentswversion =model.CurrentSwVersion,
                Newswversion =model.NewSwVersion,
                Status = model.Status,
                Creationdate = model.Creationdate,
                Creationuser = model.Creationuser,
                Modificationdate = model.Modificationdate,
                Modificationuser = model.Modificationuser,
                Deleted = model.Deleted

            };
        }
    }
}
