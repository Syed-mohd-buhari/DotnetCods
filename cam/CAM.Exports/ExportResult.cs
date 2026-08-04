using System;
using System.Text;

namespace CAM.Exports
{
    public class ExportResult
    {
        public Byte[] FileInByteArray { get; set; }

        public String ContentType { get; set; }

        public String FileName { get; set; }

        public StringBuilder Stringbuilder { get; set; }
    }
}
