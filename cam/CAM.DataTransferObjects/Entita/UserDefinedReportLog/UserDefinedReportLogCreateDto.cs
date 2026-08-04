using CAM.DataTransferObjects.Entita.Organisation;
using CAM.DataTransferObjects.LookUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.Entita.UserDefinedReportLog
{
    public class UserDefinedReportLogCreateDto : UserDefinedReportLogDto
    {
        public int Creationuser { get; set; }
        public int Modificationuser { get; set; }

    }
}
