using System.Linq;
using System.Text.RegularExpressions;
using System;

namespace CAM.WebAPI.Helper
{
    public static class CustomizeHttpRequestPath
    {
        public static string CustomizeRequestPath(string req)
        {
            string pathWithoutAction = req.TrimEnd('/').Remove(req.LastIndexOf('/') + 1);
            string actionWithIndex = req.Split('/', StringSplitOptions.RemoveEmptyEntries).Last();
            string index = Regex.Match(req, @"\d+").Value;
            string actionWithoutIndex = new String(actionWithIndex.Where(Char.IsLetter).ToArray()) + " Index:" + index;
            return pathWithoutAction + actionWithoutIndex;
        }
    }
}