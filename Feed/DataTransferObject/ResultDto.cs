namespace TEMS.DataTransferObject
{
    public class ResultDto
    {
        public bool Warning { get; set; } = false;
        public string Info { get; set; } = "";
        public object Data { get; set; }
    }
}
