using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CAM.Repository.Helpers
{
    public static class GlobalDbMode
    {
        public static Dictionary<string, string> DbMode = new Dictionary<string, string>();
        public static string CheckedDbMode { get; set; }
    }
}
