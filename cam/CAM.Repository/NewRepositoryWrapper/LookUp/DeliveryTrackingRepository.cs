using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class DeliveryTrackingRepository : RepositoryBaseNew<Deliverytrackings>, IDeliveryTrackingRepository
    {
        public DeliveryTrackingRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}
