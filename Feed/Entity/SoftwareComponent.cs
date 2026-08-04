using System.Collections.Generic;
using System.Collections;

namespace TEMS.Entity
{
    public class SoftwareComponent
    {
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
        public ArrayList? ComponentList { get; set; }
    }
}


