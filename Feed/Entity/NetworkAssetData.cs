using System;
using System.Collections;
using System.Collections.Generic;

namespace TEMS.Entity
{
    public class NetworkAssetData
    {
        
        public Dictionary<string, string> NetworkElement { get; set; } = new Dictionary<string, string>();
        public SoftwareComponent? Softwarecomponent { get; set; }
        public SoftwareConfiguration? Softconfiguration { get; set; }
        public Dictionary<string, string>? Identity { get; set; }
        public ArrayList? HardwareConfiguration { get; set; }
        public string? name { get; set; }
        public string? XmlDate { get; set; }
    }
}
