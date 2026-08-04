using CAM.Contracts.RepositoryContracts.Base;
using CAM.Entities.Models.Mail;
using OracleModels.DBModels;

namespace CAM.Contracts.RepositoryContracts.Mail
{
    public interface IMailQueueRepository: IRepositoryBase<Mailqueue>
    {
    }
}
