using CAM.DataTransferObjects.Entita.IdentityAsIs;
using CAM.DataTransferObjects.LookUp.SoftwareApplicationTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.Entita.Identity
{
    public class IdentityAsIsDtoCreate : IdentityAsIsDto
    {
        public IDictionary<short, string>? OpCoResource { get; set; }
        public IDictionary<long, string>? DcfResource { get; set; }
        public IDictionary<int, string>? CategoryResource { get; set; }
        public IDictionary<int, string>? InterfaceTypes { get; set; }
    }
}
