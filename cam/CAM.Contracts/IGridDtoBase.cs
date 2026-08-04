namespace CAM.Contracts
{
    public interface IGridDtoBase
    {
        public bool? Deleted { get; set; }
        public bool? Orphan { get; set; }
        public string LastModifiedBy { get; set; }
    }
}