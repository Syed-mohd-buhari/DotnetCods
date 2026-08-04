using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.LookUp.SubNetworkBoundary
{
    public class SubnetworkBoundaryDtoCreate : SubNetworkBoundaryGridDto
    {
        public IDictionary<long, string> DesignComponentFamiliesResource { get; set; }
        public IDictionary<long, string> DesigncomponentsResource { get; set; }
        public IDictionary<int, string> CustomerWheelResource { get; set; }
        public IDictionary<int, string> SubNetworkSupportedServices { get; set; }
        public IDictionary<short, string> SystemFunctionsResource { get; set; }
        public new short? LcmPolicy { get; set; }

        public int? VodafoneNameId { get; set; }

        public IDictionary<decimal, string>? ProductNameResource { get; set; }

        public IDictionary<int, string>? VodafoneNAmesResource { get; set; }

    }
}
