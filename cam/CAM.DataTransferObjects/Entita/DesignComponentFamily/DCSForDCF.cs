using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.Entita.DesignComponentFamily
{
    public class DCSForDCF
    {
        public string DCFName { get; set; }
        public List<DCS> DCS { get; set; }
    }
    public class DCS
    {
        public string DCName { get; set; }
        public string VodafoneName { get; set; }

    }
    public class DCFCreationModel
    {
        public long? DCFId { get; set; }
        public bool IsNewDCF { get; set; }

    }

}
