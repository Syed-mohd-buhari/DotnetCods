using CAM.Contracts.RepositoryContracts.Mail;
using CAM.Entities;
using CAM.Entities.Models.Mail;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.Mail
{
    public class MailQueueRepository : RepositoryBaseNew<Mailqueue>, IMailQueueRepository
    {
        public MailQueueRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}
