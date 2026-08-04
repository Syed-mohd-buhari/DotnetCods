namespace CAM.DataTransferObjects
{
    public class FileResultDto
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] File { get; set; }
    }
}
