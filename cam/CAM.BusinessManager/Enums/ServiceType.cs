using CAM.Contracts.RepositoryContracts.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.BusinessManager.Enums
{
    public enum ServiceType
    {
        RepositoryWrapperNew,
        RepositoryWrapper
    }

    public delegate IRepositoryWrapper ServiceResolver(ServiceType serviceType);
}
