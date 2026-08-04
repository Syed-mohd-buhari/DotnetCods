using System.Collections.Generic;

namespace CAM.Infrastucture
{

    public class FilterBaseDto<T>
    {

        public List<T> PropertyFilter { get; set; }

    }

}
