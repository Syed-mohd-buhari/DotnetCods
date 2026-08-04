using System.Collections.Generic;
using CAM.DataTransferObjects.AbstractionLayer;
using CAM.Infrastucture.QueryResult;

namespace CAM.DataTransferObjects.Grid
{
    public class SaveGrid
    {
        public List<RenderDetail> Render { get; set; }
        public string ClassName { get; set; }
        public List<UserPrefrenceDetails> UserPrefrenceDetails { get; set; }
        public int MessagingDate { get; set; }
        public int WhatsGoingOnDate { get; set; }        
        //public int Signpostdate { get; set; }
    }
}
