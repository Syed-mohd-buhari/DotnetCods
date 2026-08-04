using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects
{
    public class ReferenceTableDto
    {
        string ReferenceTableName { get; set; }
        long ReferencePrimaryKey { get; set; }

        long ReferenceKey { get; set; }
    }
}
