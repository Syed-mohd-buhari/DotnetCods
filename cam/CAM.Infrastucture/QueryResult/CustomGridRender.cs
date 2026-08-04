using System.Collections.Generic;

namespace CAM.Infrastucture.QueryResult
{
    public class CustomGridRender<T>
    {
      
        public string ClassName = typeof(T).Name;
        public List<RenderDetail> Render { get; set; }
    }


}