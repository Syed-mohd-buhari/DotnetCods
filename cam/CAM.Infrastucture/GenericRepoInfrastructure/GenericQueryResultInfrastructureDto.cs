using System.Collections.Generic;

namespace CAM.Infrastucture.QueryResult
{
    public class GenericQueryResultInfrastructureDto<T>
    {
        public GenericQueryResultInfrastructureDto(IGenericReportInfrastructureGrid generateReport,long id)
        {
            GridRender = generateReport.GenerateReport<T>(id);
        }

        public GenericQueryResultInfrastructureDto()
        {

        }

        public int TotalItems { get; set; }
        public IList<T> Items { get; set; }
        public GenericReportInfrastructureGrid<T> GridRender { get; set; }
    }
}
