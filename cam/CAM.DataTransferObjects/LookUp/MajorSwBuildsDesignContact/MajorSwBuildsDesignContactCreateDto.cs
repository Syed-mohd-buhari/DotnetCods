using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.LookUp.MajorSwBuildsDesignContact
{
    public class MajorSwBuildsDesignContactCreateDto : MajorSwBuildsDesignContactDto
    {
        public IDictionary<int, string> DesignContacts { get; set; }
    }
}
