namespace CAM.DataTransferObjects.AbstractionLayer
{
    public class UserPrefrenceDetails
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Path { get; set; }
        public string Menu { get; set; }
        public int? Order { get; set; }
        public bool Default { get; set; }

        public int? ScreenId { get; set; }
        public int? ScreenPermission { get; set; }
    }
    public class DoingSectioinForPA : PlanndActivitySoftwareUpgradDetailsDtoGrid
    {
        public string colour { get; set; }

    }
    public class AspnetuserPreferenceCreateOrUpdateDto
    {
        public int Aspnetuserpreferenceid { get; set; }
        public int Userid { get; set; }
        public int Modelid { get; set; }
        public short Permission {  get; set; }
        public int Order { get; set; }
    }
}
