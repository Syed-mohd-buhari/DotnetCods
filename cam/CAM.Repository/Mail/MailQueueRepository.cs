using CAM.Contracts.RepositoryContracts.Mail;
using CAM.Entities;
using CAM.Entities.Models.Mail;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.Mail
{
    public class MailQueueRepository : RepositoryBase<Mailqueue>, IMailQueueRepository
    {
        public MailQueueRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
