using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using CAM.Entities.Models.Mail;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Mail
{
    public static class MailQueueMapper
    {
        public static MailQueue GetMailQueueMapper(Mailqueue model)
        {
            if (model == null)
                return null;
            return new MailQueue()
            {
                MailId =model.Mailid,
                ApprovalUserId = model.Approvaluserid,
                Body =model.Body,
                ErrorMessage = model.Errormessage,
                From =model.From,
                State =(MailStateEnum)model.State,
                Subject =model.Subject,
                To =model.To,
                ApprovalUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.Approvaluser),
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                
            };
        }
        public static Mailqueue SetMailQueueMapper(MailQueue model)
        {
            return new Mailqueue()
            {

                Mailid = model.MailId,
                Approvaluserid = model.ApprovalUserId,
                Body = model.Body,
                Errormessage = model.ErrorMessage,
                From = model.From,
                State = (byte)model.State,
                Subject = model.Subject,
                To = model.To,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
            };
        }
    }
}
