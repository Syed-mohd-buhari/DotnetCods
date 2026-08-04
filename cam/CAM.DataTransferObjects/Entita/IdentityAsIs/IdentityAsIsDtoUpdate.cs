using CAM.DataTransferObjects.LookUp.Type;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.Identity
{
    public class IdentityAsIsDtoUpdate : IdentityAsIsDtoCreate
    {
        public IDictionary<int, string>? ClassResource { get; set; }
        public IDictionary<int, string>? TypeResource { get; set; }
        public IDictionary<long, string>? AssetResource { get; set; }
        public short OpcoId { get; set; }
        public long? DcfId { get; set; }

    }
}
