using System;
using System.Collections.Generic;
using System.Text;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.DataTransferObjects.VIA;
using CAM.Infrastucture.QueryResult;

namespace CAM.BusinessManager.Entity.Vai
{
  public  class QueryResultDtoVai : QueryResultDto<ViaExport>
    {
        public QueryResultDtoVai(GenerateRenderForGrid<ViaExport> generateRenderForGrid):base(generateRenderForGrid)
        {
            
        }
        //public int SkipAmount { get; set; }
        //public List<SkipManager> SkipManager { get; set; }

    }
}
