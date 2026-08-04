namespace CAM.DataTransferObjects
{
    public class ResultDto
    {
        public bool Warning { get; set; } = false;
        public string Info { get; set; } = "";
        public object Data { get; set; }
    }
    public class ResultDto<T>
    {
        public bool Warning { get; set; } = false;
        public string Info { get; set; } = "";
        public T Data { get; set; }
    }
}