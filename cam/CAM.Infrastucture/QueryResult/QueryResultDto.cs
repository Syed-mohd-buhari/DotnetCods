using System.Collections.Generic;

namespace CAM.Infrastucture.QueryResult
{
    public class QueryResultDto<T>
    {
        public QueryResultDto(IGenerateRender generateRender)
        {
            GridRender = generateRender.GenerateRender<T>();
        }

        public QueryResultDto()
        {

        }

        public int TotalItems { get; set; }
        public IList<T> Items { get; set; }
        public CustomGridRender<T> GridRender { get; set; }
    }
}
