using System.Collections;
using System.Collections.Generic;

namespace TEMS.Entity
{
    public class SoftwareConfiguration
    {
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
        public ArrayList? FunctionList { get; set; }
        public SubFunction? SubFunction { get; set; }
    }
}
