using System.Collections.Generic;
using System.Collections;

namespace TEMS.Entity
{
    public class FunctionArea
    {
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
        public ArrayList? Subfunction_List { get; set; }
    }
}
