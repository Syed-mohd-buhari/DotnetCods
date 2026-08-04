using System.Collections.Generic;
using System.Collections;

namespace TEMS.Entity
{
    public class SubFunctionArea
    {
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
        public ArrayList? SubFunctionAreaList { get; set; }
    }
}
